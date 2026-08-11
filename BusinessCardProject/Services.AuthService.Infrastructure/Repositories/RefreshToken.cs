using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Dtos.DTO.Auth;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Repositories
{
    public class RefreshToken : IRefreshToken
    {
        private readonly IAuthDbContext _context;

        public RefreshToken(IAuthDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Save(string refreshToken, string userId, Instant expiresAt, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("Refresh token cannot be empty", nameof(refreshToken));
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            if (expiresAt <= SystemClock.Instance.GetCurrentInstant())
                throw new ArgumentException("Expiration date must be in the future", nameof(expiresAt));

            /*TODO: Борьба с мульти-устройствами --> возможно надо убрать*/
            var oldTokens = await _context.RefreshToken
                .Where(x => x.UserId == Guid.Parse(userId)).ToListAsync(cancellationToken: ct);
            
            foreach (var oldToken in oldTokens) oldToken.ChangeIsRevoked(true);
            {
                _context.RefreshToken.UpdateRange(oldTokens);
            }
            
            var tokenHash = HashToken(refreshToken);
            var newRefreshToken = new RefreshTokenEntity(
                tokenHash: tokenHash,
                userId: Guid.Parse(userId),
                createdAtUtc: SystemClock.Instance.GetCurrentInstant(),
                expiresAtUtc: expiresAt,
                isRevoked: false
            );
            
            _context.RefreshToken.RemoveRange(oldTokens.Where(t => t.IsRevoked));
            await _context.RefreshToken.AddAsync(newRefreshToken, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<RefreshTokenInfo?> GetAndInvalidate(string refreshTokenRequest, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenRequest)) return null;

            var tokenHash = HashToken(refreshTokenRequest);
            var refreshToken = await _context.RefreshToken
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);

            if (refreshToken == null || refreshToken.IsRevoked ||
                refreshToken.ExpiresAtUtc < SystemClock.Instance.GetCurrentInstant())
            {
                return null;
            }

            refreshToken.ChangeIsRevoked(true);
            _context.RefreshToken.Update(refreshToken);
            await _context.SaveChangesAsync(ct);

            return new RefreshTokenInfo(
                UserId: refreshToken.UserId.ToString(),
                ExpiresAt: refreshToken.ExpiresAtUtc,
                IsRevoked: true
            );
        }

        public async Task<RefreshTokenInfo?> CheckOfExpireRefreshToken(string refreshToken, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken)) return null;

            var tokenHash = HashToken(refreshToken);
            var refreshTokenEntity = await _context.RefreshToken
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);

            if (refreshTokenEntity == null || refreshTokenEntity.IsRevoked ||
                refreshTokenEntity.ExpiresAtUtc < SystemClock.Instance.GetCurrentInstant())
            {
                return null;
            }

            return new RefreshTokenInfo(
                UserId: refreshTokenEntity.UserId.ToString(),
                ExpiresAt: refreshTokenEntity.ExpiresAtUtc,
                IsRevoked: false
            );
        }

        public async Task Revoke(string refreshToken, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken)) return;

            var tokenHash = HashToken(refreshToken);
            var refreshTokenEntity = await _context.RefreshToken.FindAsync([tokenHash], cancellationToken: ct);

            if (refreshTokenEntity != null && !refreshTokenEntity.IsRevoked)
            {
                refreshTokenEntity.ChangeIsRevoked(true);
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RevokeAllForUser(string userId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userId)) return;

            var userGuid = Guid.Parse(userId);
            await _context.RefreshToken
                .Where(t => t.UserId == userGuid && !t.IsRevoked)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(t => t.IsRevoked, true)
                        .SetProperty(t => t.ExpiresAtUtc, SystemClock.Instance.GetCurrentInstant()),
                    ct);
        }

        public async Task<IReadOnlyList<SessionInfo>> GetActiveSessions(string accessToken, int limit, CancellationToken ct = default)
        {
            var userId = await GetUserIdFromAccessToken(accessToken, ct);
            var user = await _context.User.FindAsync([userId], ct)
                       ?? throw new Exception("Пользователь не найден");
            var dateTimeNow = SystemClock.Instance.GetCurrentInstant();
            
            return await _context.RefreshToken
                .AsNoTracking()
                .Where(t => t.UserId == user.Id && !t.IsRevoked && t.ExpiresAtUtc > dateTimeNow)
                .OrderByDescending(t => t.CreatedAtUtc)
                .Take(limit)
                .Select(t => new SessionInfo(t.TokenHash, t.CreatedAtUtc, t.ExpiresAtUtc))
                .ToListAsync(ct);
        }

        public async Task<Guid> GetUserIdFromAccessToken(string accessToken, CancellationToken ct = default)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(accessToken))
            {
                throw new Exception("Не удалось считать токен");
            }

            var jwt = handler.ReadJwtToken(accessToken);
            var userName = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var user = await _context.User.FirstOrDefaultAsync(x => x.NickName == userName, ct)
                       ?? throw new Exception("Пользователь не найден");
            
            return user.Id;
        }

        /// <summary>
        /// Хэширует токен с помощью SHA256 для безопасного хранения.
        /// </summary>
        private static string HashToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token cannot be empty", nameof(token));
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hashBytes);
        }
    }
}