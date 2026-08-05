using AuthorizationService.Proto;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using NodaTime;
using Requests.User;
using Services.AuthService.Application.Application.Command;
using Services.AuthService.Application.Application.Extensions;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Domain.Enums;
using Services.AuthService.Presentation.ProtoMappers.Users;
using CreateUserRequest = AuthorizationService.Proto.CreateUserRequest;
using LoginRequest = AuthorizationService.Proto.LoginRequest;
using UpdateUserRequest = AuthorizationService.Proto.UpdateUserRequest;

namespace Services.AuthService.Presentation.Services
{
    [Authorize]
    public class AuthService : AuthorizationService.Proto.AuthorizationService.AuthorizationServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IRefreshToken _refreshToken;
        private readonly IUserRepository _users;
        private readonly IAccountVerificationRepository _accountVerifications;
        private readonly IFailedLoginAttemptRepository _failedLoginAttempts;
        private readonly IAuditLogRepository _auditLog;
        private readonly IUserSettingsRepository _userSettings;

        private readonly ILogger<AuthService> _logger;

        private TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(15);

        public AuthService(IMediator mediator, IRefreshToken refreshToken, IUserRepository users,
            IAccountVerificationRepository accountVerifications, ILogger<AuthService> logger,
            IFailedLoginAttemptRepository failedLoginAttempts, IAuditLogRepository auditLog,
            IUserSettingsRepository userSettings)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _accountVerifications = accountVerifications ??
                                    throw new ArgumentNullException(nameof(accountVerifications));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _failedLoginAttempts = failedLoginAttempts ?? throw new ArgumentNullException(nameof(failedLoginAttempts));
            _auditLog = auditLog ?? throw new ArgumentNullException(nameof(auditLog));
            _userSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
        }

        [AllowAnonymous]
        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            try
            {
                var user = await _users.GetUserByEmail(request.Email)
                           ?? throw new RpcException(new Status(StatusCode.Unauthenticated,
                               "Invalid or expired email"));

                if (await _failedLoginAttempts.IsLockedOut(user.Id))
                {
                    _logger.LogWarning("Пользователь {UserId} заблокирован по причине превышения попыток входа",
                        user.Id);
                    throw new RpcException(new Status(StatusCode.ResourceExhausted,
                        "Аккаунт временно заблокирован. Попробуйте позже"));
                }

                if (!await _users.VerifyPassword(user.Id, request.Password))
                {
                    await _failedLoginAttempts.RecordAttempt(user.Id);
                    var failures = await _failedLoginAttempts.GetConsecutiveFailures(
                        user.Id, SystemClock.Instance.GetCurrentInstant().Minus(Duration.FromHours(24)));

                    if (failures >= 5)
                    {
                        throw new RpcException(new Status(StatusCode.ResourceExhausted,
                            "Аккаунт временно заблокирован. Попробуйте позже"));
                    }

                    throw new RpcException(new Status(StatusCode.Unauthenticated, "Неверный email или пароль"));
                }

                await _failedLoginAttempts.ClearAttempts(user.Id);

                if (!await _users.CheckVerificationAcc(user.Id))
                {
                    throw new RpcException(new Status(StatusCode.Unauthenticated, "Account not verified"));
                }

                var accessToken = await _mediator.Send(new TokenGenerateAccessTokenCommand(user));
                var refreshToken = await _mediator.Send(new TokenGenerateRefreshTokenCommand());

                await _refreshToken.Save(refreshToken, user.Id.ToString(),
                    SystemClock.Instance.GetCurrentInstant() + Duration.FromDays(1));

                /*TODO: Сделать потом везде, чтобы можно было проводить аудит действий пользователя*/
                var ipAddress = context.Peer;
                var userAgent = context.RequestHeaders
                    .FirstOrDefault(x => x.Key.Equals("user-agent", StringComparison.OrdinalIgnoreCase))?.Value ?? "";

                await _auditLog.Log(user.Id, nameof(Login), $"Успещный вход с IP - {ipAddress}", ipAddress, userAgent);

                return new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = (int)AccessTokenLifetime.TotalSeconds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(Login));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [AllowAnonymous]
        public override async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request,
            ServerCallContext context)
        {
            try
            {
                var tokenInfo = await _refreshToken.GetAndInvalidate(request.RefreshToken);
                if (tokenInfo == null || tokenInfo.IsExpired)
                {
                    throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or expired refresh token"));
                }

                var user = await _users.GetUser(Guid.TryParse(tokenInfo.UserId, out var userId)
                    ? userId
                    : throw new RpcException(new Status(StatusCode.Unauthenticated,
                        "Invalid or expired refresh token")));

                var newAccessToken = await _mediator.Send(new TokenGenerateAccessTokenCommand(user));
                var newRefreshToken = await _mediator.Send(new TokenGenerateRefreshTokenCommand());

                await _refreshToken.Save(newRefreshToken, user.Id.ToString(),
                    SystemClock.Instance.GetCurrentInstant() + Duration.FromDays(1));

                return new RefreshTokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresIn = (int)AccessTokenLifetime.TotalSeconds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(RefreshToken));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [AllowAnonymous]
        public override async Task<LogoutResponse> Logout(LogoutRequest request, ServerCallContext context)
        {
            try
            {
                await _refreshToken.Revoke(request.RefreshToken);
                return new LogoutResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(Logout));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [AllowAnonymous]
        public override async Task<VerificationAccountResponse> VerificationAccount(VerificationAccountRequest request,
            ServerCallContext context)
        {
            try
            {
                var result = await _accountVerifications
                    .VerificationRecord(request.UserId.ToGuid(), request.VerificationCode);

                return new VerificationAccountResponse
                {
                    Success = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(VerificationAccount));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [AllowAnonymous]
        public override async Task<UserBooleanResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            try
            {
                var newUser = await _users.CreateUser(new()
                {
                    Surname = request.Surname,
                    Name = request.Name,
                    NickName = request.NickName,
                    Email = request.Email,
                    Password = request.Password
                });

                return new()
                {
                    Success = newUser
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(CreateUser));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<UserBooleanResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            try
            {
                var updateUser = await _users.UpdateUser(new(request.UserId.ToGuid(),
                    request.Surname, request.Name, request.NickName, request.Email));
                return new()
                {
                    Success = updateUser
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<UserInfoResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            try
            {
                var user = await _users.GetUser(request.UserId.ToGuid());
                return new()
                {
                    UserId = user.Id.ToString(),
                    Surname = user.Surname,
                    Name = user.Name,
                    Nickname = user.NickName,
                    Email = user.Email,
                    IsAuthor = user.IsAuthor,
                    CreatedAt = user.CreatedAt.ToTimestamp(),
                    UpdatedAt = user.UpdatedAt?.ToTimestamp() ?? null,
                    DeletedAt = user.DeletedAt?.ToTimestamp() ?? null
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [AllowAnonymous]
        public override async Task<UserInfoResponse> GetUserByEmail(GetUserByEmailRequest request,
            ServerCallContext context)
        {
            try
            {
                var user = await _users.GetUserByEmail(request.UserEmail);
                return new()
                {
                    UserId = user.Id.ToString(),
                    Surname = user.Surname,
                    Name = user.Name,
                    Nickname = user.NickName,
                    Email = user.Email,
                    IsAuthor = user.IsAuthor,
                    CreatedAt = user.CreatedAt.ToTimestamp(),
                    UpdatedAt = user.UpdatedAt?.ToTimestamp() ?? null,
                    DeletedAt = user.DeletedAt?.ToTimestamp() ?? null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(GetUserByEmail));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<AddAuthorRoleResponse> AddAuthorRole(AddAuthorRoleRequest request,
            ServerCallContext context)
        {
            try
            {
                var response = await _users.AddAuthorRole(request.UserId.ToGuid());
                return new()
                {
                    Success = response
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<GetUsersFromSearchResponse> GetUsersFromSearch(GetUsersFromSearchRequest request,
            ServerCallContext context)
        {
            try
            {
                var response = await _users.GetUsersFromSearch(new GetUsersSearchRequest(request.Page, request.PageSize,
                    request.Search, request.SortBy, request.SortDirection));
                return response.Items.ToProtoUsersFromSearchInfoList();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [AllowAnonymous]
        public override async Task<UserBooleanResponse> SendPasswordReset(SendPasswordResetRequest request,
            ServerCallContext context)
        {
            try
            {
                await _mediator.Send(new SendPasswordResetCommand(request.Email));
                return new UserBooleanResponse { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка при отправке письма сброса пароля для {Email}", request.Email);
                return new UserBooleanResponse { Success = true };
            }
        }

        [AllowAnonymous]
        public override async Task<UserBooleanResponse> ResetPassword(ResetPasswordRequest request,
            ServerCallContext context)
        {
            try
            {
                await _mediator.Send(new ResetPasswordCommand(request.UserId.ToGuid(), request.ResetToken,
                    request.NewPassword));
                return new UserBooleanResponse { Success = true };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Aborted, ex.Message));
            }
        }

        [Authorize]
        public override async Task<LogoutAllResponse> LogoutAll(LogoutAllRequest request, ServerCallContext context)
        {
            try
            {
                var userId = Guid.Parse(request.UserId);
                await _refreshToken.RevokeAllForUser(userId.ToString());

                return new LogoutAllResponse { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выходе со всех устройств");
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [Authorize]
        public override async Task<ActiveSessionsResponse> GetActiveSessions(GetActiveSessionsRequest request,
            ServerCallContext context)
        {
            try
            {
                var accessToken = !string.IsNullOrEmpty(request.AccessToken)
                    ? request.AccessToken
                    : throw new Exception("Пользователь не авторизован");
                var sessions =
                    await _refreshToken.GetActiveSessions(accessToken, request.Limit, context.CancellationToken);

                var response = new ActiveSessionsResponse();
                foreach (var s in sessions)
                {
                    response.Sessions.Add(new ActiveSessionInfo
                    {
                        TokenId = s.TokenHash,
                        CreatedAt = s.CreatedAtUtc.ToTimestamp(),
                        ExpiresAt = s.ExpiresAtUtc.ToTimestamp()
                    });
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении активных сессий");
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [AllowAnonymous]
        public override async Task<ValidateRefreshTokenResponse> ValidateRefreshToken(RefreshTokenRequest request,
            ServerCallContext context)
        {
            try
            {
                var tokenInfo =
                    await _refreshToken.CheckOfExpireRefreshToken(request.RefreshToken, context.CancellationToken);
                return new ValidateRefreshTokenResponse { IsValid = tokenInfo != null };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при валидации refresh токена");
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [Authorize]
        public override async Task<UserInfoResponse> GetCurrentUser(GetCurrentUserRequest request,
            ServerCallContext context)
        {
            try
            {
                var userId = await _refreshToken.GetUserIdFromAccessToken(request.AccessToken);
                var user = await _users.GetUser(userId)
                           ?? throw new RpcException(new Status(StatusCode.NotFound, "Пользователь не найден"));

                return new UserInfoResponse
                {
                    UserId = user.Id.ToString(),
                    Surname = user.Surname,
                    Name = user.Name,
                    Nickname = user.NickName,
                    Email = user.Email,
                    IsAuthor = user.IsAuthor,
                    CreatedAt = user.CreatedAt.ToTimestamp(),
                    UpdatedAt = user.UpdatedAt?.ToTimestamp() ?? null,
                    DeletedAt = user.DeletedAt?.ToTimestamp() ?? null,
                    IsVerified = user.IsVerified
                };
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(GetCurrentUser));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [Authorize]
        public override async Task<UserSettingsResponse> GetUserSettings(GetUserSettingsRequest request,
            ServerCallContext context)
        {
            try
            {
                var userId = await _refreshToken.GetUserIdFromAccessToken(request.AccessToken);
                var settings = await _userSettings.GetByUserId(userId);

                return new UserSettingsResponse
                {
                    JsonSettings = settings?.JsonSettings ?? "{}",
                    Success = true
                };
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(GetUserSettings));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [Authorize]
        public override async Task<UserSettingsResponse> SaveUserSettings(UserSettingsRequest request,
            ServerCallContext context)
        {
            try
            {
                var userId = await _refreshToken.GetUserIdFromAccessToken(request.AccessToken);
                await _userSettings.Save(userId, request.JsonSettings);
                var settings = await _userSettings.GetByUserId(userId);

                return new UserSettingsResponse
                {
                    JsonSettings = settings?.JsonSettings ?? "{}",
                    Success = true
                };
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(SaveUserSettings));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }
    }
}