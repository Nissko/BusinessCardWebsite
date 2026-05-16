using Dtos.DTO.User;
using Requests.User;

namespace ByteCodePlatform.Application.Common.Interfaces.Repositories
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
        /// Выдача авторских прав
        /// </summary>
        Task<UserAuthorDto> CreateAuthorUser(CreateAuthorRequest request);

        /// <summary>
        /// Получение пользователя
        /// </summary>
        Task<UserDto> GetUser(Guid userId);
    }
}