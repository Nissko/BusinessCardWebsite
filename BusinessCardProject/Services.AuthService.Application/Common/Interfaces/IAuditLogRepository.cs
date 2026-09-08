using DTOs.DTO.Auth;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IAuditLogRepository
    {
        /// <summary>
        /// 
        /// </summary>
        Task Log(Guid userId, string action, string? details = null, string? ipAddress = null, string? userAgent = null,
            CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        Task<IReadOnlyList<AuditLogDto>> GetLogsForUser(Guid userId, int page, int pageSize,
            CancellationToken ct = default);
    }
}