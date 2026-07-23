namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IAccountVerificationRepository
    {
        /// <summary>
        /// Добавить новую запись
        /// </summary>
        Task<bool> CreateRecord(Guid userId);

        /// <summary>
        /// Подтверждение аккаунта
        /// </summary>
        Task<bool> VerificationRecord(Guid userId, string verificationCode);
        
        /// <summary>
        /// Проверка прохождения верификации у пользователя
        /// </summary>
        Task<bool> CheckVerification(Guid userId);
    }
}