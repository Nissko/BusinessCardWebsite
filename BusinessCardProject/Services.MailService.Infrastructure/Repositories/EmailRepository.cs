using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RequestModels.Email;
using Services.MailService.Application.Common.Interfaces;
using Services.MailService.Infrastructure.Settings;

namespace Services.MailService.Infrastructure.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly MailSettings _settings;
        private readonly MailDbContext _context;
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailRepository(IOptions<MailSettings> options, MailDbContext context,
            IEmailTemplateService emailTemplateService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _emailTemplateService =
                emailTemplateService ?? throw new ArgumentNullException(nameof(emailTemplateService));
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<bool> SendAsync(EmailMessageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var mail = new MimeMessage();

                var senderName = string.IsNullOrEmpty(request.DisplayName)
                    ? _settings.DisplayName
                    : request.DisplayName;
                var senderEmail = string.IsNullOrEmpty(request.From)
                    ? _settings.From
                    : request.From;

                mail.From.Add(new MailboxAddress(senderName, senderEmail));
                mail.Sender = new MailboxAddress(senderName, senderEmail);

                foreach (var to in request.To.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mail.To.Add(MailboxAddress.Parse(to));
                }

                foreach (var cc in request.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mail.Cc.Add(MailboxAddress.Parse(cc));
                }

                foreach (var bcc in request.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mail.Bcc.Add(MailboxAddress.Parse(bcc));
                }

                if (!string.IsNullOrEmpty(request.ReplyTo))
                {
                    mail.ReplyTo.Add(new MailboxAddress(request.ReplyToName
                                                        ?? "Reply", request.ReplyTo));
                }

                mail.Subject = request.Subject;
                var bodyBuilder = new BodyBuilder();

                if (request.IsHtml)
                {
                    var template = _context.EmailContentTemplate.FirstOrDefault(x =>
                        x.TemplateName == request.TemplateName);

                    if (template != null)
                    {
                        bodyBuilder.HtmlBody = await _emailTemplateService
                            .GetTemplateBody(template.TemplateName, template.Body, request);
                    }
                    else
                    {
                        bodyBuilder.HtmlBody = request.Body;
                    }
                }
                else
                {
                    bodyBuilder.TextBody = request.Body;
                }

                mail.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                var secureOption = _settings.UseSsl
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTls;

                await client.ConnectAsync(_settings.Host, _settings.Port, secureOption, cancellationToken);
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(_settings.UserName, _settings.Password, cancellationToken);
                await client.SendAsync(mail, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}