using Grpc.Core;
using RequestModels.Email;
using Services.MailService.Application.Common.Interfaces;
using SmtpMailService.Proto;

namespace Services.MailService.Presentation.Services
{
    public class EmailGrpcService(IEmailRepository emailRepository) : SmtpMailGrpcService.SmtpMailGrpcServiceBase
    {
        private readonly IEmailRepository _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));

        public override async Task<SendEmailResponse> SendEmail(SendEmailRequest request, ServerCallContext context)
        {
            try
            {
                var sendEmail = await _emailRepository.SendAsync(new EmailMessageRequest(request.To, request.Cc,
                        request.Bcc, request.From, request.DisplayName, request.ReplyTo, request.ReplyToName,
                        request.Subject, request.Body, request.IsHtml,
                        request.TemplateName),
                    CancellationToken.None);
                
                return new()
                {
                    IsSuccess =  sendEmail
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}
