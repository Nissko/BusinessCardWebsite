using Dtos.DTO.Auth;
using NodaTime;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IRefreshToken
    {
        /// <summary>
        /// 
        /// </summary>
        Task Save(string refreshToken, string userId, Instant expiresAt, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        Task<RefreshTokenInfo?> GetAndInvalidate(string refreshToken, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        Task Revoke(string refreshToken, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        Task RevokeAllForUser(string userId, CancellationToken ct = default);

        /// <summary>
        /// 
        /// </summary>
        Task<IReadOnlyList<SessionInfo>> GetActiveSessions(string accessToken, int limit,
            CancellationToken ct = default);
    }
}