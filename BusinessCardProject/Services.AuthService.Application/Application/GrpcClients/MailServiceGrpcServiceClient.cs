using Grpc.Core;
using Requests.Email;
using Services.AuthService.Application.Common.Interfaces.GrpcClients;
using SmtpMailService.Proto;

namespace Services.AuthService.Application.Application.GrpcClients
{
    /// <summary>
    /// Клиент Grpc сервиса почты
    /// </summary>
    public class MailServiceGrpcServiceClient : IMailServiceGrpcServiceClient
    {
        private readonly SmtpMailGrpcService.SmtpMailGrpcServiceClient _client;

        public MailServiceGrpcServiceClient(SmtpMailGrpcService.SmtpMailGrpcServiceClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<bool> SendEmailMessageAsync(EmailMessageRequest request)
        {
            try
            {
                var response = await _client.SendEmailAsync(new SendEmailRequest
                {
                    To = { request.To },
                    Cc = { request.Cc },
                    Bcc = { request.Bcc },
                    From = request.From,
                    DisplayName = request.DisplayName,
                    ReplyTo = request.ReplyTo,
                    ReplyToName = request.ReplyToName,
                    Subject = request.Subject,
                    Body = request.Body,
                    IsHtml = request.IsHtml,
                    TemplateName = request.TemplateName
                });

                return response.IsSuccess;
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}