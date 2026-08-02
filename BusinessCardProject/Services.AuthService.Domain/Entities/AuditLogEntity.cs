using NodaTime;
using Services.AuthService.Domain.Common;

namespace Services.AuthService.Domain.Entities
{
    public class AuditLogEntity : Entity
    {
        public AuditLogEntity(Guid userId, string action, string? details = null, string? ipAddress = null,
            string? userAgent = null)
        {
            UserId = userId;
            Action = action;
            Details = details;
            CreatedAt = SystemClock.Instance.GetCurrentInstant();
            IpAddress = ipAddress ?? "";
            UserAgent = userAgent ?? "";
        }

        public Guid UserId { get; private set; }
        public string Action { get; private set; }
        public string? Details { get; private set; }
        public Instant CreatedAt { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }
    }
}