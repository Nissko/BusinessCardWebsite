using Grpc.Core;
using Services.MailService.Application.Common.Interfaces;
using Services.MailService.Domain.Models;
using SmtpMailService.Proto;

namespace Services.MailService.Presentation.Services
{
    public class EmailGrpcService : SmtpMailGrpcService.SmtpMailGrpcServiceBase
    {
        private readonly IEmailSender _emailSender;

        public EmailGrpcService(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public override async Task<SendEmailResponse> SendEmail(SendEmailRequest request, ServerCallContext context)
        {
            try
            {
                var sendEmail = await _emailSender.SendAsync(new EmailMessageRequest(request.To, request.Cc,
                    request.Bcc, request.From, request.DisplayName, request.ReplyTo, request.ReplyToName,
                    request.Subject, request.Body, request.IsHtml), CancellationToken.None);
                
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
