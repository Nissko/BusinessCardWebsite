using NodaTime;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IFailedLoginAttemptRepository
    {
        /// <summary>
        /// Добавление записи об неудачной попытке входа
        /// </summary>
        Task RecordAttempt(Guid userId, CancellationToken ct = default);

        /// <summary>
        /// Проверяет на блокировку пользователя
        /// </summary>
        Task<bool> IsLockedOut(Guid userId, CancellationToken ct = default);

        /// <summary>
        /// Очистка истории
        /// </summary>
        Task ClearAttempts(Guid userId, CancellationToken ct = default);

        /// <summary>
        /// Возврат кол-ва неудачных последовательных попыток 
        /// </summary>
        Task<int> GetConsecutiveFailures(Guid userId, Instant windowStart, CancellationToken ct = default);
    }
}