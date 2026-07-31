using Dtos.DTO.Auth;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IAuditLogRepository
    {
        Task Log(Guid userId, string action, string? details = null, CancellationToken ct = default);
        Task<IReadOnlyList<AuditLogDto>> GetLogsForUser(Guid userId, int page, int pageSize, CancellationToken ct = default);
    }
}