using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace Services.AuthService.Domain.Entities
{
    public class RefreshTokenEntity
    {
        public RefreshTokenEntity(string? tokenHash, Guid? userId, Instant createdAtUtc,
            Instant expiresAtUtc, bool isRevoked, string? userAgent)
        {
            TokenHash = tokenHash;
            UserId = userId;
            CreatedAtUtc = createdAtUtc;
            ExpiresAtUtc = expiresAtUtc;
            IsRevoked = isRevoked;
            UserAgent = userAgent;
        }

        [Key] 
        [MaxLength(64)] 
        public string? TokenHash { get; private set; }

        public Guid? UserId { get; private set; }
        public Instant CreatedAtUtc { get; private set; }
        public Instant ExpiresAtUtc { get; private set; }
        public bool IsRevoked { get; private set; }
        [MaxLength(512)]
        public string? UserAgent { get; private set; }

        public void ChangeIsRevoked(bool value)
        {
            IsRevoked = value;
        }
    }
}