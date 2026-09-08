using MediatR;
using Microsoft.Extensions.Configuration;
using RequestModels.Email;
using Services.AuthService.Application.Application.Command;
using Services.AuthService.Application.Common.Interfaces.GrpcClients;

namespace Services.AuthService.Application.Application.CommandHandlers
{
    /// <summary>
    /// Универсальная отправка уведомлений через сервис уведомлений
    /// </summary>
    public class SendMailNotificationCommandHandler : IRequestHandler<SendMailNotificationCommand, bool>
    {
        private readonly IMailServiceGrpcServiceClient _mailServiceGrpcServiceClient;
        private readonly IConfiguration _configuration;

        public SendMailNotificationCommandHandler(IMailServiceGrpcServiceClient mailServiceGrpcServiceClient, IConfiguration configuration)
        {
            _mailServiceGrpcServiceClient = mailServiceGrpcServiceClient ??
                                            throw new ArgumentNullException(nameof(mailServiceGrpcServiceClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(SendMailNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var fromEmail = _configuration.GetValue<string>("MailSettings:From") ??
                                throw new Exception("Настройки почты From не найдена");
                var displayNameEmail = _configuration.GetValue<string>("MailSettings:DisplayName") ??
                                  throw new Exception("Настройки почты DisplayName не найдена");

                var result = await _mailServiceGrpcServiceClient.SendEmailMessageAsync(new EmailMessageRequest(
                    To: request.To ?? [],
                    Cc: request.Cc ?? [],
                    Bcc: request.Bcc ?? [],
                    From: request.From ?? fromEmail,
                    DisplayName: displayNameEmail,
                    ReplyTo: request.ReplyTo ?? "",                           
                    ReplyToName: request.ReplyToName ?? "",                       
                    Subject: request.Subject,   
                    Body: request.Body,   
                    IsHtml: request.IsHtml,
                    TemplateName: request.TemplateName ?? null
                ));
                
                return result;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}