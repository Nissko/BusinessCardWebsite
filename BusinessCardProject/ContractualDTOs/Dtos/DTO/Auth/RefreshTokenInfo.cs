using NodaTime;

namespace Dtos.DTO.Auth
{
    public record RefreshTokenInfo(string? UserId, Instant ExpiresAt, bool IsRevoked)
    {
        public bool IsExpired => SystemClock.Instance.GetCurrentInstant() > ExpiresAt;
    };
}