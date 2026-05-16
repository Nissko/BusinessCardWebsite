using ByteCodePlatform.Application.Application.Extensions;
using ByteCodePlatform.Application.Common.Interfaces.Repositories;
using Grpc.Core;
using UserService.Proto;

namespace ByteCodePlatform.API.Services
{
    public class UserGrpcService : UserService.Proto.UserGrpcService.UserGrpcServiceBase
    {
        private readonly IUserRepository _userService;

        public UserGrpcService(IUserRepository userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public override async Task<UserBooleanResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            try
            {
                var newUser = await _userService.CreateUser(
                    new(request.Surname, request.Name, request.NickName, request.Email));
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

        public override async Task<UserBooleanResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            try
            {
                var updateUser = await _userService.UpdateUser(new(request.UserId.ToGuid(),
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
                        UserId = createAuthor.UserInfo.Id.ToString(),
                        Surname = createAuthor.UserInfo.Surname,
                        Name = createAuthor.UserInfo.Name,
                        Nickname = createAuthor.UserInfo.NickName,
                        Email = createAuthor.UserInfo.Email,
                        IsAuthor = createAuthor.UserInfo.IsAuthor,
                        CreatedAt = createAuthor.UserInfo.CreatedAt.ToTimestamp(),
                        UpdatedAt = createAuthor.UserInfo.UpdatedAt?.ToTimestamp() ?? null,
                        DeletedAt = createAuthor.UserInfo.DeletedAt?.ToTimestamp() ?? null
                    }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<UserInfoResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            try
            {
                var user = await _userService.GetUser(request.UserId.ToGuid());
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