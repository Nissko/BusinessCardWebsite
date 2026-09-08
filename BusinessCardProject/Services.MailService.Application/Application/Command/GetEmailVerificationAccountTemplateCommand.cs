using MediatR;
using RequestModels.Email;

namespace Services.MailService.Application.Application.Command
{
    public class GetEmailVerificationAccountTemplateCommand(
        string templateBody,
        EmailMessageRequest emailMessageRequest)
        : IRequest<string>
    {
        /// <summary>
        /// Тело из шаблона
        /// </summary>
        public string TemplateBody { get; } = templateBody;

        /// <summary>
        /// Данные которые подставляем в шаблон
        /// </summary>
        public EmailMessageRequest EmailMessageRequest { get; } = emailMessageRequest;
    }
}