using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IPasswordResetRepository
    {
        /// <summary>
        /// Создание новой запись для сброса пароля
        /// </summary>
        Task<string> CreateRecord(Guid userId, CancellationToken ct = default);

        /// <summary>
        /// Получение действительной записи сброса пароля
        /// </summary>
        Task<PasswordResetEntity?> GetValidRecord(Guid userId, string token, CancellationToken ct = default);

        /// <summary>
        /// Для пометки, что запись была использована
        /// </summary>
        Task MarkAsUsed(Guid userId, string token, CancellationToken ct = default);

        /// <summary>
        /// Удаление всех записей с таблицы
        /// </summary>
        Task RevokeAllForUser(Guid userId, CancellationToken ct = default);
    }
}