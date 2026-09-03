using GlobalEnums.EmailNotifications;
using MediatR;
using Microsoft.Extensions.Configuration;
using Services.AuthService.Application.Application.Command;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Domain.Entities;
using SystemClock = NodaTime.SystemClock;

namespace Services.AuthService.Infrastructure.Repositories
{
    public class AccountVerificationRepository : IAccountVerificationRepository
    {
        private readonly IAuthDbContext _context;
        private readonly IMediator _mediator;
        private readonly string _clientUrl;
        private const string EmailSubject = "Подтверждение аккаунта";
        
        private List<string> _emailRecipients = new();

        public AccountVerificationRepository(IAuthDbContext context, IMediator mediator, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            var configurationSetting = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _clientUrl = configurationSetting.GetValue<string>("FrontendLink:Url") ??
                         "https://it-bytecode.splinterkeenetic.netcraze.club";
        }

        public async Task<bool> CreateRecord(Guid userId)
        {
            var user = await _context.User.FindAsync([userId]) ?? throw new Exception("Пользователь не найден");
            if (user.VerifyMail) return false;
            if (user.UserVerifications.Count != 0)
            {
                var verificationRecord = user.UserVerifications.FirstOrDefault();
                if (verificationRecord != null) _context.AccountVerification.Remove(verificationRecord);
            }

            var record = AddNewVerificationRecord(userId);
            await _context.SaveChangesAsync(CancellationToken.None);

            //Отправка сообщения на почту
            _emailRecipients.Add(user.Email);
            await SendMailNotification(_emailRecipients,
                $"{_clientUrl}/verification-account/{record.UserId}/{record.VerificationToken}",
                nameof(EmailTemplatesEnum.VerificationAccountTemplate));
            
            return true;
        }

        public async Task<bool> VerificationRecord(Guid userId, string verificationCode)
        {
            var dateTimeNow = SystemClock.Instance.GetCurrentInstant();
            var dateTimeUtc =  dateTimeNow.ToDateTimeUtc();
            var user = await _context.User.FindAsync([userId]) ?? throw new Exception("Пользователь не найден");
            var verificationRecord = user.UserVerifications.FirstOrDefault()
                                     ?? throw new Exception("Запись верификации не найдена");

            if (user.VerifyMail) throw new Exception("Учетная запись уже подтверждена");
            if (dateTimeUtc > verificationRecord.ExpiresAt.ToDateTimeUtc() ||
                !verificationRecord.VerificationToken.Equals(verificationCode))
            {
                return false;
            }
            
            verificationRecord.SetVerificationAt(dateTimeNow);
            user.SetVerified();
            
            _context.AccountVerification.Update(verificationRecord);
            _context.User.Update(user);
            
            await _context.SaveChangesAsync(CancellationToken.None);
            
            return true;
        }
        
        /// <summary>
        /// TODO: добавить расширенный вывод (Дата истечения, чтобы сделать красивый UI)
        /// </summary>
        public async Task<bool> CheckVerification(Guid userId)
        {
            return (await _context.User.FindAsync([userId]))!.VerifyMail;
        }

        private AccountActivationEntity AddNewVerificationRecord(Guid userId)
        {
            var token = Guid.NewGuid().ToString("N");
            var verificationRecord = new AccountActivationEntity(userId, token);
            
            _context.AccountVerification.Add(verificationRecord);
            
            return verificationRecord;
        }

        /// <summary>
        /// Отправка сообщения на почту
        /// </summary>
        private Task<bool> SendMailNotification(List<string> emailRecipients, string message, string templateName)
        {
            return _mediator.Send(new SendMailNotificationCommand(subject: EmailSubject,
                body: message,
                to: emailRecipients,
                templateName: templateName));
        }
    }
}