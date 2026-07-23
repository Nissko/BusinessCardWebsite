using NodaTime;
using Services.AuthService.Domain.Common;

namespace Services.AuthService.Domain.Entities
{
    public class AccountActivationEntity : Entity
    {
        public AccountActivationEntity(Guid userId, string verificationToken)
        {
            UserId = userId;
            VerificationToken = verificationToken;
            CreatedAt = SystemClock.Instance.GetCurrentInstant();
            ExpiresAt = CreatedAt.Plus(Duration.FromMinutes(15));
        }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; private set; }
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// Токен верификации
        /// </summary>
        public string VerificationToken { get; private set; }

        /// <summary>
        /// Время создания записи
        /// </summary>
        public Instant CreatedAt { get; private set; }

        /// <summary>
        /// Время истечения срока
        /// </summary>
        public Instant ExpiresAt { get; private set; }

        /// <summary>
        /// Время подтверждения
        /// </summary>
        public Instant? VerificationAt { get; private set; }

        /// <summary>
        /// Установка времени прохождения верификации
        /// </summary>
        public void SetVerificationAt(Instant dateTime) => VerificationAt = dateTime;
    }
}