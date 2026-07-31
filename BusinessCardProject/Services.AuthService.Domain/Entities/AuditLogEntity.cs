using NodaTime;
using Services.AuthService.Domain.Common;

namespace Services.AuthService.Domain.Entities
{
    public class AuditLogEntity : Entity
    {
        public AuditLogEntity(Guid userId, string action, string? details = null)
        {
            UserId = userId;
            Action = action;
            Details = details;
            CreatedAt = SystemClock.Instance.GetCurrentInstant();
            IpAddress = "";
            UserAgent = "";
        }

        public Guid UserId { get; private set; }
        public string Action { get; private set; }
        public string? Details { get; private set; }
        public Instant CreatedAt { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }

        public void UpdateContext(string ipAddress, string userAgent)
        {
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }
    }
}