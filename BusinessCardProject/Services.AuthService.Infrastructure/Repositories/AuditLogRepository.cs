using Dtos.DTO.Auth;
using Microsoft.EntityFrameworkCore;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly IAuthDbContext _context;

        public AuditLogRepository(IAuthDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Log(Guid userId, string action, string? details = null, CancellationToken ct = default)
        {
            var log = new AuditLogEntity(userId, action, details);
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<AuditLogDto>> GetLogsForUser(Guid userId, int page, int pageSize, CancellationToken ct = default)
        {
            var skip = (page - 1) * pageSize;
            return await _context.AuditLogs
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .Select(l => new AuditLogDto(
                    l.UserId, l.Action, l.Details, l.CreatedAt, l.IpAddress))
                .ToListAsync(ct);
        }
    }
}