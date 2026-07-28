using Services.MailService.Domain.Common;

namespace Services.MailService.Domain.Entities
{
    public class EmailContentTemplateEntity : Entity
    {
        public EmailContentTemplateEntity(string templateName, string subject, string body)
        {
            TemplateName = templateName;
            Subject = subject;
            Body = body;
        }

        /// <summary>
        /// Название шаблона
        /// </summary>
        public string TemplateName { get; }

        /// <summary>
        /// Название темы из шаблона
        /// </summary>
        public string Subject { get; private set; }

        /// <summary>
        /// Тело письма
        /// </summary>
        public string Body { get; private set; }
    }
}