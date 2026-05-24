using Dtos.DTO.User;
using Requests.User;

namespace ByteCodePlatform.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        Task<bool> CreateUser(Guid userId);
        
        /// <summary>
        /// Выдача авторских прав
        /// </summary>
        Task<UserAuthorDto> CreateAuthorUser(CreateAuthorRequest request);
    }
}