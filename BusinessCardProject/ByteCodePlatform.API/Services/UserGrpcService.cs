using ByteCodePlatform.API.ProtoMappers.Course.Users;
using ByteCodePlatform.Application.Application.Extensions;
using ByteCodePlatform.Application.Common.Interfaces.Repositories;
using ByteCodePlatform.Domain.Enums;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using RequestModels.User;
using UserService.Proto;
using CreateAuthorRequest = UserService.Proto.CreateAuthorRequest;
using CreateUserRequest = UserService.Proto.CreateUserRequest;

namespace ByteCodePlatform.API.Services
{
    [Authorize]
    public class UserGrpcService : UserService.Proto.UserGrpcService.UserGrpcServiceBase
    {
        private readonly IUserRepository _userService;
        private readonly ILogger<UserGrpcService> _logger;

        public UserGrpcService(IUserRepository userService, ILogger<UserGrpcService> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [AllowAnonymous]
        public override async Task<UserBooleanResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            try
            {
                var newUser = await _userService.CreateUser(request.UserId.ToGuid());
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
        public override async Task<UserAuthorInfoResponse> CreateAuthorUser(CreateAuthorRequest request,
            ServerCallContext context)
        {
            try
            {
                var createAuthor = await _userService.CreateAuthorUser(new(request.UserId.ToGuid()));
                return new()
                {
                    AuthorId = createAuthor.AuthorId.ToString(),
                    User = new()
                    {
                        UserId = createAuthor.UserInfo.Id.ToString()
                    }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<GetUsersFromSearchResponse> GetUsersFromSearch(GetUsersFromSearchRequest request, ServerCallContext context)
        {
            try
            {
                var usersFromSearch = await _userService.GetUsersFromSearch(
                    new GetUsersSearchRequest(request.Page, request.PageSize, request.Search, request.SortBy,
                        request.SortDirection));

                return usersFromSearch.Items.ToProtoUsersFromSearchInfoList();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [Authorize]
        public override async Task<ChangeAuthorAvatarResponse> ChangeAuthorAvatar(ChangeAuthorAvatarRequest request, ServerCallContext context)
        {
            try
            {
                var isUpdateAuthorAvatar = await _userService.UpdateAuthorAvatar(request.UserId.ToGuid(), request.AvatarId);
                return new ChangeAuthorAvatarResponse { Result = isUpdateAuthorAvatar };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}