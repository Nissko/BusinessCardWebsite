using NodaTime;

namespace DTOs.DTO.Auth
{
    public record RefreshTokenInfo(string? UserId, Instant ExpiresAt, bool IsRevoked, bool NeedsRenewal = false)
    {
        public bool IsExpired => SystemClock.Instance.GetCurrentInstant() > ExpiresAt;
    }
}