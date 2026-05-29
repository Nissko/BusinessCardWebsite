using AuthorizationService.Proto;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using NodaTime;
using Services.AuthService.Application.Application.Command;
using Services.AuthService.Application.Application.Extensions;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Domain.Enums;

namespace Services.AuthService.Presentation.Services
{
    [Authorize]
    public class AuthService : AuthorizationService.Proto.AuthorizationService.AuthorizationServiceBase
    {
        private readonly IMediator _mediator;
        private readonly IRefreshToken _refreshToken;
        private readonly IUserRepository _users;
        private TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(15);

        public AuthService(IMediator mediator, IRefreshToken refreshToken, IUserRepository users)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _refreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        [AllowAnonymous]
        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            try
            {
                var user = await _users.GetUserByEmail(request.Email) ??
                           throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or expired email"));
                if (!await _users.VerifyPassword(user.Id, request.Password))
                    throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid credentials"));

                var accessToken = await _mediator.Send(new TokenGenerateAccessTokenCommand(user));
                var refreshToken = await _mediator.Send(new TokenGenerateRefreshTokenCommand());

                await _refreshToken.SaveAsync(refreshToken, user.Id.ToString(),
                    SystemClock.Instance.GetCurrentInstant() + Duration.FromDays(1));

                return new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = (int)AccessTokenLifetime.TotalSeconds
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [AllowAnonymous]
        public override async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request,
            ServerCallContext context)
        {
            try
            {
                var tokenInfo = await _refreshToken.GetAndInvalidateAsync(request.RefreshToken);
                if (tokenInfo == null || tokenInfo.IsExpired)
                    throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or expired refresh token"));

                var user = await _users.GetUser(Guid.TryParse(tokenInfo.UserId, out var userId)
                    ? userId
                    : throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or expired refresh token")));
            
                var newAccessToken = await _mediator.Send(new TokenGenerateAccessTokenCommand(user));
                var newRefreshToken = await _mediator.Send(new TokenGenerateRefreshTokenCommand());

                await _refreshToken.SaveAsync(newRefreshToken, user.Id.ToString(),
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
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
        
        [AllowAnonymous]
        public override async Task<LogoutResponse> Logout(LogoutRequest request, ServerCallContext context)
        {
            try
            {
                await _refreshToken.RevokeAsync(request.RefreshToken);
                return new LogoutResponse();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
        
        [AllowAnonymous]
        public override async Task<UserBooleanResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            try
            {
                var newUser = await _users.CreateUser(new(request.Surname, request.Name, request.NickName,
                    request.Email, request.Password));
                return new()
                {
                    Success = newUser
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
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

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<UserInfoResponse> GetUserByEmail(GetUserByEmailRequest request, ServerCallContext context)
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
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}