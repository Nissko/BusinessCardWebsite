using Dtos.DTO.Auth;
using NodaTime;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IRefreshToken
    {
        Task SaveAsync(string refreshToken, string userId, Instant expiresAt, CancellationToken ct = default);
        Task<RefreshTokenInfo?> GetAndInvalidateAsync(string refreshToken, CancellationToken ct = default);
        Task RevokeAsync(string refreshToken, CancellationToken ct = default);
        Task RevokeAllForUserAsync(string userId, CancellationToken ct = default);
        Task<IReadOnlyList<SessionInfo>> GetActiveSessionsAsync(Guid userId, int limit, CancellationToken ct = default);
    }
}