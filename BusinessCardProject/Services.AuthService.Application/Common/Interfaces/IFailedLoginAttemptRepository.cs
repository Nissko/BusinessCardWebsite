using NodaTime;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IFailedLoginAttemptRepository
    {
        Task RecordAttemptAsync(Guid userId, CancellationToken ct = default);
        Task<bool> IsLockedOutAsync(Guid userId, CancellationToken ct = default);
        Task ClearAttemptsAsync(Guid userId, CancellationToken ct = default);
        Task<int> GetConsecutiveFailuresAsync(Guid userId, Instant windowStart, CancellationToken ct = default);
    }
}