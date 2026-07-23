using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Requests.Email;
using Services.MailService.Application.Common.Interfaces;
using Services.MailService.Infrastructure.Settings;

namespace Services.MailService.Infrastructure.Repositories
{
    public class EmailRepository : IEmailSender
    {
        private readonly MailSettings _settings;

        public EmailRepository(IOptions<MailSettings> options)
        {
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<bool> SendAsync(EmailMessageRequest messageRequest, CancellationToken cancellationToken)
        {
            try
            {
                var mail = new MimeMessage();

                var senderName = string.IsNullOrEmpty(messageRequest.DisplayName)
                    ? _settings.DisplayName
                    : messageRequest.DisplayName;
                var senderEmail = string.IsNullOrEmpty(messageRequest.From)
                    ? _settings.From
                    : messageRequest.From;

                mail.From.Add(new MailboxAddress(senderName, senderEmail));
                mail.Sender = new MailboxAddress(senderName, senderEmail);

                foreach (var to in messageRequest.To.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mail.To.Add(MailboxAddress.Parse(to));
                }

                foreach (var cc in messageRequest.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mail.Cc.Add(MailboxAddress.Parse(cc));
                }

                foreach (var bcc in messageRequest.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    mail.Bcc.Add(MailboxAddress.Parse(bcc));
                }

                if (!string.IsNullOrEmpty(messageRequest.ReplyTo))
                {
                    mail.ReplyTo.Add(new MailboxAddress(messageRequest.ReplyToName
                                                        ?? "Reply", messageRequest.ReplyTo));
                }

                mail.Subject = messageRequest.Subject;
                var bodyBuilder = new BodyBuilder();

                if (messageRequest.IsHtml)
                {
                    bodyBuilder.HtmlBody = messageRequest.Body;
                }
                else
                {
                    bodyBuilder.TextBody = messageRequest.Body;
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