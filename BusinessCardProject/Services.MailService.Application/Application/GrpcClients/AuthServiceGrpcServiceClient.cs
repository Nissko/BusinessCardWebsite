using AuthorizationService.Proto;
using Grpc.Core;
using Services.MailService.Application.Common.Interfaces.GrpcClients;

namespace Services.MailService.Application.Application.GrpcClients
{
    public class AuthServiceGrpcServiceClient(
        AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient authServiceGrpcServiceClient)
        : IAuthServiceGrpcServiceClient
    {
        private readonly AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient
            _authServiceGrpcServiceClient = authServiceGrpcServiceClient ??
                                            throw new ArgumentNullException(nameof(authServiceGrpcServiceClient));

        public async Task<string> GetUserNameFromEmail(string email)
        {
            try
            {
                var result = await _authServiceGrpcServiceClient.GetUserByEmailAsync(new GetUserByEmailRequest
                {
                    UserEmail = email
                });
                return result.Name;
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}