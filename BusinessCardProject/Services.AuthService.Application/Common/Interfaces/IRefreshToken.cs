using Dtos.DTO.Auth;
using NodaTime;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IRefreshToken
    {
        /// <summary>
        /// Сохранение нового refresh token для пользователя
        /// </summary>
        Task Save(string refreshToken, string userId, Instant expiresAt, CancellationToken ct = default);

        /// <summary>
        /// Получение информации о токене обновления и немедленное аннулирование.
        /// Используется для предотвращения повторного использования
        /// </summary>
        Task<RefreshTokenInfo?> GetAndInvalidate(string refreshToken, CancellationToken ct = default);

        /// <summary>
        /// Проверка токена на истечение или принудительного отзыва
        /// </summary>
        Task<RefreshTokenInfo?> CheckOfExpireRefreshToken(string refreshToken, CancellationToken ct = default);

        /// <summary>
        /// Аннулирование определенного токена
        /// </summary>
        Task Revoke(string refreshToken, CancellationToken ct = default);

        /// <summary>
        /// Аннулирование всех активных токенов обновления для пользователя.
        /// Для logout all и при смене пароля
        /// </summary>
        Task RevokeAllForUser(string userId, CancellationToken ct = default);

        /// <summary>
        /// Получение списка активных сессий пользователя
        /// </summary>
        Task<IReadOnlyList<SessionInfo>> GetActiveSessions(string accessToken, int limit,
            CancellationToken ct = default);

        /// <summary>
        /// Получение идентификатора пользователя по его токену
        /// </summary>
        Task<Guid> GetUserIdFromAccessToken(string accessToken, CancellationToken ct = default);
    }
}