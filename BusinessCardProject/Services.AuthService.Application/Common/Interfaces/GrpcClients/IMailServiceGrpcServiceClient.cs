using Requests.Email;

namespace Services.AuthService.Application.Common.Interfaces.GrpcClients
{
    public interface IMailServiceGrpcServiceClient
    {
        /// <summary>
        /// Отправка нового сообщения в SMTP
        /// </summary>
        Task<bool> SendEmailMessageAsync(EmailMessageRequest request);
    }
}