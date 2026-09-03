namespace Services.AuthService.Application.Common.Interfaces.GrpcClients
{
    public interface ICoreGrpcServiceClient
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        Task<bool> CreateUser(Guid userId);
        
        /// <summary>
        /// Обновление аватара у автора
        /// </summary>
        Task<bool> UpdateUserAuthorAvatar(Guid userId, string avatarId);
    }
}