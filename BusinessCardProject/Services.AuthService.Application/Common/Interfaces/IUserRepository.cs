using Dtos.DTO.Pagination;
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

        /// <summary>
        /// Проверка верификации аккаунта
        /// </summary>
        Task<bool> CheckVerificationAcc(Guid userId);

        /// <summary>
        /// Список ролей пользователя
        /// </summary>
        Task<List<string>> GetRoles(Guid userId);
        
        /// <summary>
        /// Добавление роли автора из ядра
        /// </summary>
        Task<bool> AddAuthorRole(Guid userId);
        
        /// <summary>
        /// Получение всех пользователей для админки
        /// </summary>
        Task<PaginationDto<UserDto>> GetUsersFromSearch(GetUsersSearchRequest request);
        
        Task<bool> UpdatePassword(Guid userId, string passwordHash);
    }
}