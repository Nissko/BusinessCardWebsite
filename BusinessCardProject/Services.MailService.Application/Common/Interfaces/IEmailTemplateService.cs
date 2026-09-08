using RequestModels.Email;

namespace Services.MailService.Application.Common.Interfaces
{
    public interface IEmailTemplateService
    {
        Task<string> GetTemplateBody(string templateName, string templateBody, EmailMessageRequest emailMessageRequest);
    }
}