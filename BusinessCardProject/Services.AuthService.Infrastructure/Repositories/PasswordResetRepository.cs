using Microsoft.EntityFrameworkCore;
using NodaTime;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly IAuthDbContext _context;

        public PasswordResetRepository(IAuthDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateRecordAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await _context.User.FindAsync([userId], ct)
                       ?? throw new Exception("Пользователь не найден");

            var existing = await _context.PasswordResets
                .Where(r => r.UserId == userId && !r.IsUsed && r.ExpiresAt > SystemClock.Instance.GetCurrentInstant())
                .ToListAsync(ct);

            if (existing.Any())
            {
                _context.PasswordResets.RemoveRange(existing);
                await _context.SaveChangesAsync(ct);
            }

            var token = Guid.NewGuid().ToString("N");
            var record = new PasswordResetEntity(userId, token);
            _context.PasswordResets.Add(record);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<PasswordResetEntity?> GetValidRecordAsync(Guid userId, string token,
            CancellationToken ct = default)
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            return await _context.PasswordResets
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ResetToken == token && !r.IsUsed && r.ExpiresAt > now,
                    ct);
        }

        public async Task MarkAsUsedAsync(Guid userId, string token, CancellationToken ct = default)
        {
            var record = await _context.PasswordResets
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ResetToken == token, ct);

            if (record != null)
            {
                record.MarkAsUsed();
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default)
        {
            var records = await _context.PasswordResets
                .Where(r => r.UserId == userId && !r.IsUsed)
                .ToListAsync(ct);

            _context.PasswordResets.RemoveRange(records);
            await _context.SaveChangesAsync(ct);
        }
    }
}