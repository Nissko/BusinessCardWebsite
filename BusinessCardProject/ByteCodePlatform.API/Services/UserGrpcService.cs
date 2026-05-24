using ByteCodePlatform.Application.Application.Extensions;
using ByteCodePlatform.Application.Common.Interfaces.Repositories;
using ByteCodePlatform.Domain.Enums;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using UserService.Proto;

namespace ByteCodePlatform.API.Services
{
    [Authorize]
    public class UserGrpcService : UserService.Proto.UserGrpcService.UserGrpcServiceBase
    {
        private readonly IUserRepository _userService;

        public UserGrpcService(IUserRepository userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
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
    }
}