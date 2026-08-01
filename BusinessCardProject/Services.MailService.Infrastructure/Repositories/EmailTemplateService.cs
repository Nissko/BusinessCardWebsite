using GlobalEnums.EmailNotifications;
using MediatR;
using Requests.Email;
using Services.MailService.Application.Application.Command;
using Services.MailService.Application.Common.Interfaces;

namespace Services.MailService.Infrastructure.Repositories
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IMediator _mediator;

        public EmailTemplateService(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<string> GetTemplateBody(string templateName, string templateBody, EmailMessageRequest emailMessageRequest)
        {
            switch (templateName)
            {
                case nameof(EmailTemplatesEnum.VerificationAccountTemplate):
                {
                    return await _mediator.Send(new GetEmailVerificationAccountTemplateCommand(
                        templateBody, emailMessageRequest));
                }
            }
            
            // В случае если не был найден шаблон, то просто возвращаю исходную строку из запроса
            return emailMessageRequest.Body;
        }
    }
}