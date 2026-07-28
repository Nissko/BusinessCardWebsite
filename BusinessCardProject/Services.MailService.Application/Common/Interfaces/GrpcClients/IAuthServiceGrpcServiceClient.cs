namespace Services.MailService.Application.Common.Interfaces.GrpcClients
{
    public interface IAuthServiceGrpcServiceClient
    {
        /// <summary>
        /// Получение имени пользователя по его почте
        /// </summary>
        Task<string> GetUserNameFromEmail(string email);
    }
}