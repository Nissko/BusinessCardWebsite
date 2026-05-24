namespace Services.AuthService.Application.Common.Interfaces.GrpcClients
{
    public interface IUserCoreGrpcService
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        Task<bool> CreateUser(Guid userId);
    }
}