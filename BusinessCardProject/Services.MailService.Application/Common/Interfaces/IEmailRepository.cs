using RequestModels.Email;

namespace Services.MailService.Application.Common.Interfaces
{
    public interface IEmailRepository
    {
        /// <summary>
        /// Отправка сообщения пользователю
        /// </summary>
        Task<bool> SendAsync(EmailMessageRequest messageRequest, CancellationToken cancellationToken);
    }
}