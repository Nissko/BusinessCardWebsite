using Grpc.Core;
using Services.AuthService.Application.Common.Interfaces.GrpcClients;
using UserService.Proto;

namespace Services.AuthService.Application.Application.GrpcClients
{
    public class CoreUserServiceClient : IUserCoreGrpcService
    {
        private readonly UserGrpcService.UserGrpcServiceClient  _userGrpcService;

        public CoreUserServiceClient(UserGrpcService.UserGrpcServiceClient userGrpcService)
        {
            _userGrpcService = userGrpcService ?? throw new ArgumentNullException(nameof(userGrpcService));
        }

        public async Task<bool> CreateUser(Guid userId)
        {
            try
            {
                var newUser = await _userGrpcService.CreateUserAsync(new CreateUserRequest
                {
                    UserId = userId.ToString()
                });

                return newUser.Success;
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}