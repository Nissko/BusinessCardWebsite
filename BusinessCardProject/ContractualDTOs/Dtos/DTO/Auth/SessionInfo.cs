using NodaTime;

namespace DTOs.DTO.Auth
{
    public record SessionInfo(string? TokenHash, Instant CreatedAtUtc, Instant ExpiresAtUtc, string? UserAgent);
}