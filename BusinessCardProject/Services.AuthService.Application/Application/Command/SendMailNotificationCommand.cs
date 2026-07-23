using MediatR;

namespace Services.AuthService.Application.Application.Command
{
    public class SendMailNotificationCommand : IRequest<bool>
    {
        public SendMailNotificationCommand(
            string subject,
            string body,
            IReadOnlyList<string>? to = null,
            IReadOnlyList<string>? cc = null,
            IReadOnlyList<string>? bcc = null,
            string? from = null,
            string? replyTo = null,
            string? replyToName = null,
            string? templateName = null)
        {
            Subject = subject;
            Body = body;
            To = to;
            Cc = cc;
            Bcc = bcc;
            From = from;
            ReplyTo = replyTo;
            ReplyToName = replyToName;
            IsHtml = !string.IsNullOrEmpty(templateName);
            TemplateName = templateName;
        }

        /// <summary>
        /// Список основных получателей
        /// </summary>
        public IReadOnlyList<string>? To { get; }

        /// <summary>
        /// Список получателей копии письма
        /// </summary>
        public IReadOnlyList<string>? Cc { get; }

        /// <summary>
        /// Список получателей, получающих скрытую копию письма
        /// </summary>
        public IReadOnlyList<string>? Bcc { get; }

        /// <summary>
        /// Отправитель
        /// </summary>
        public string? From { get; }

        /// <summary>
        /// Кому будут отправляться ответы
        /// </summary>
        public string? ReplyTo { get; }

        /// <summary>
        /// Отображаемое имя для адреса ответа
        /// </summary>
        public string? ReplyToName { get; }

        /// <summary>
        /// Тема письма
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// Тело письма
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Признак того, что тело письма в формате HTML
        /// </summary>
        public bool IsHtml { get; }

        /// <summary>
        /// Название шаблона
        /// </summary>
        public string? TemplateName { get; }
    }
}