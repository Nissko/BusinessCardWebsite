using Services.MailService.Domain.Models;

namespace Services.MailService.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        /// <summary>
        /// Отправка сообщения пользователю
        /// </summary>
        Task<bool> SendAsync(EmailMessageRequest messageRequest, CancellationToken cancellationToken);
    }
}