using NodaTime;

namespace Dtos.DTO.Auth
{
    public record SessionInfo(string TokenHash, Instant CreatedAtUtc, Instant ExpiresAtUtc);
}