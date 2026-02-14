namespace BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.User
{
    public interface IPasswordHash
    {
        /// <summary>
        /// Хэширование пароля
        /// </summary>
        /// <param name="password">Введенный пользователем пароль</param>
        Task<string> HashPassword(string password);
    
        /// <summary>
        /// Верификация пароля
        /// </summary>
        /// <param name="hashedPassword">Пароль из БД</param>
        /// <param name="providedPassword">Пароль из Requst(-a)</param>
        /// <returns></returns>
        Task<bool> VerifyPassword(string hashedPassword, string providedPassword);
    }
}