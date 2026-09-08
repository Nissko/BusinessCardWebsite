using NodaTime;

namespace DTOs.DTO.Auth
{
    public record AuditLogDto(
        Guid UserId,
        string Action,
        string? Details,
        Instant CreatedAt,
        string IpAddress,
        string UserAgent);
}