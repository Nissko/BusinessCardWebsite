using Dtos.DTO.Auth;
using NodaTime;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IRefreshToken
    {
        Task Save(string refreshToken, string userId, Instant expiresAt, CancellationToken ct = default);
        Task<RefreshTokenInfo?> GetAndInvalidate(string refreshToken, CancellationToken ct = default);
        Task Revoke(string refreshToken, CancellationToken ct = default);
        Task RevokeAllForUser(string userId, CancellationToken ct = default);
        Task<IReadOnlyList<SessionInfo>> GetActiveSessions(string accessToken, int limit, CancellationToken ct = default);
    }
}