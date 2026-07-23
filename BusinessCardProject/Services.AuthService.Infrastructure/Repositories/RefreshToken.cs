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

        public async Task SaveAsync(string refreshToken, string userId, Instant expiresAt, CancellationToken ct = default)
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
            _context.RefreshToken.UpdateRange(oldTokens);
            
            var tokenHash = HashToken(refreshToken);
            var newRefreshToken = new RefreshTokenEntity(
                tokenHash: tokenHash,
                userId: Guid.Parse(userId),
                createdAtUtc: SystemClock.Instance.GetCurrentInstant(),
                expiresAtUtc: expiresAt,
                isRevoked: false
            );

            await _context.RefreshToken.AddAsync(newRefreshToken, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<RefreshTokenInfo?> GetAndInvalidateAsync(string refreshToken, CancellationToken ct = default)
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

            refreshTokenEntity.ChangeIsRevoked(true);
            _context.RefreshToken.Update(refreshTokenEntity);
            await _context.SaveChangesAsync(ct);

            return new RefreshTokenInfo(
                UserId: refreshTokenEntity.UserId.ToString(),
                ExpiresAt: refreshTokenEntity.ExpiresAtUtc,
                IsRevoked: true
            );
        }

        public async Task RevokeAsync(string refreshToken, CancellationToken ct = default)
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

        public async Task RevokeAllForUserAsync(string userId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userId)) return;

            var userGuid = Guid.Parse(userId);
            var affected = await _context.RefreshToken
                .Where(t => t.UserId == userGuid && !t.IsRevoked)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(t => t.IsRevoked, true)
                        .SetProperty(t => t.ExpiresAtUtc, SystemClock.Instance.GetCurrentInstant()),
                    ct);
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