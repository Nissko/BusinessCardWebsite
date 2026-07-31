using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NodaTime;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Repositories
{
    public class FailedLoginAttemptRepository : IFailedLoginAttemptRepository
    {
        private readonly IAuthDbContext _context;
        private readonly int _maxAttempts;
        private readonly TimeSpan _lockoutDuration;
        private readonly TimeSpan _cleanupWindow;

        public FailedLoginAttemptRepository(IAuthDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _maxAttempts = configuration.GetValue<int>("AuthSettings:MaxFailedAttempts", 5);
            _lockoutDuration = TimeSpan.FromMinutes(configuration.GetValue<double>("AuthSettings:LockoutMinutes", 15));
            _cleanupWindow = TimeSpan.FromHours(configuration.GetValue<double>("AuthSettings:CleanupWindowHours", 24));
        }

        public async Task RecordAttemptAsync(Guid userId, CancellationToken ct = default)
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            var windowStart = now.Minus(Duration.FromTicks(_cleanupWindow.Ticks));

            var oldAttempts = _context.FailedLoginAttempts
                .Where(x => x.UserId == userId && x.AttemptedAt < windowStart);
            _context.FailedLoginAttempts.RemoveRange(oldAttempts);

            var recentCount = await _context.FailedLoginAttempts
                .CountAsync(x => x.UserId == userId && x.AttemptedAt >= windowStart, ct);

            if (recentCount < _maxAttempts)
            {
                _context.FailedLoginAttempts.Add(new FailedLoginAttemptEntity(userId));
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task<bool> IsLockedOutAsync(Guid userId, CancellationToken ct = default)
        {
            var now = SystemClock.Instance.GetCurrentInstant();
            var lockoutStart = now.Minus(Duration.FromTicks(_lockoutDuration.Ticks));

            var count = await _context.FailedLoginAttempts
                .CountAsync(x => x.UserId == userId && x.AttemptedAt >= lockoutStart, ct);

            return count >= _maxAttempts;
        }

        public async Task ClearAttemptsAsync(Guid userId, CancellationToken ct = default)
        {
            var attempts = await _context.FailedLoginAttempts
                .Where(x => x.UserId == userId)
                .ToListAsync(ct);

            _context.FailedLoginAttempts.RemoveRange(attempts);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<int> GetConsecutiveFailuresAsync(Guid userId, Instant windowStart, CancellationToken ct = default)
        {
            return await _context.FailedLoginAttempts
                .CountAsync(x => x.UserId == userId && x.AttemptedAt >= windowStart, ct);
        }
    }
}