using MediatR;
using Services.MailService.Application.Application.Command;
using Services.MailService.Application.Common.Interfaces.GrpcClients;

namespace Services.MailService.Application.Application.CommandHandlers
{
    public class GetEmailVerificationAccountTemplateCommandHandler(
        IAuthServiceGrpcServiceClient authServiceGrpcServiceClient)
        : IRequestHandler<GetEmailVerificationAccountTemplateCommand, string>
    {
        private readonly IAuthServiceGrpcServiceClient _authServiceGrpcServiceClient = authServiceGrpcServiceClient ??
            throw new ArgumentNullException(nameof(authServiceGrpcServiceClient));

        public async Task<string> Handle(GetEmailVerificationAccountTemplateCommand request,
            CancellationToken cancellationToken)
        {
            if (request.EmailMessageRequest.To.FirstOrDefault() == null || request == null)
            {
                throw new Exception("Неверные данные о получателе");
            }

            var userName = await _authServiceGrpcServiceClient
                .GetUserNameFromEmail(request.EmailMessageRequest.To.FirstOrDefault()
                                      ?? throw new Exception("Неверные данные о получателе"));

            return request.TemplateBody
                .Replace("{CompanyName}", "БайтКод")
                .Replace("{UserName}", userName)
                .Replace("{ConfirmationLink}", request.EmailMessageRequest.Body);
        }
    }
}