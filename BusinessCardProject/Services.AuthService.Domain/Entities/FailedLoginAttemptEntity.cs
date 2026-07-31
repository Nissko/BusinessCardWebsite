using NodaTime;
using Services.AuthService.Domain.Common;

namespace Services.AuthService.Domain.Entities
{
    public class FailedLoginAttemptEntity : Entity
    {
        public FailedLoginAttemptEntity(Guid userId)
        {
            UserId = userId;
            AttemptedAt = SystemClock.Instance.GetCurrentInstant();
        }

        public Guid UserId { get; private set; }
        public Instant AttemptedAt { get; private set; }
    }
}