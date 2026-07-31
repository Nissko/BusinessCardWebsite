using NodaTime;
using Services.AuthService.Domain.Common;

namespace Services.AuthService.Domain.Entities
{
    public class PasswordResetEntity : Entity
    {
        public PasswordResetEntity(Guid userId, string resetToken)
        {
            UserId = userId;
            ResetToken = resetToken;
            CreatedAt = SystemClock.Instance.GetCurrentInstant();
            ExpiresAt = CreatedAt.Plus(Duration.FromMinutes(30));
            IsUsed = false;
        }

        public Guid UserId { get; private set; }
        public string ResetToken { get; private set; }
        public Instant CreatedAt { get; private set; }
        public Instant ExpiresAt { get; private set; }
        public bool IsUsed { get; private set; }

        public void MarkAsUsed()
        {
            IsUsed = true;
        }
    }
}