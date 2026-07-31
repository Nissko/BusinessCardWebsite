using NodaTime;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IFailedLoginAttemptRepository
    {
        Task RecordAttempt(Guid userId, CancellationToken ct = default);
        Task<bool> IsLockedOut(Guid userId, CancellationToken ct = default);
        Task ClearAttempts(Guid userId, CancellationToken ct = default);
        Task<int> GetConsecutiveFailures(Guid userId, Instant windowStart, CancellationToken ct = default);
    }
}