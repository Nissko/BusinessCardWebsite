using Dtos.DTO.User;
using Requests.User;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        Task<bool> CreateUser(CreateUserRequest request);

        /// <summary>
        /// Изменение пользовательских данных
        /// </summary>
        Task<bool> UpdateUser(UpdateUserRequest request);

        /// <summary>
        /// Получение пользователя
        /// </summary>
        Task<UserDto> GetUser(Guid userId);
        
        /// <summary>
        /// Поиск пользователя по почте
        /// </summary>
        Task<UserDto> GetUserByEmail(string email);
        
        Task<bool> VerifyPassword(Guid userId, string plainPassword);

        Task<List<string>> GetRoles(Guid userId);
    }
}