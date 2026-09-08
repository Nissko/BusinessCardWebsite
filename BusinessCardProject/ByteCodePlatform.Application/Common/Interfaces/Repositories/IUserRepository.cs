using DTOs.DTO.Pagination;
using DTOs.DTO.User;
using RequestModels.User;

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
        
        /// <summary>
        /// Получение всех пользователей для админки
        /// </summary>
        Task<PaginationDto<UserDto>> GetUsersFromSearch(GetUsersSearchRequest request);

        /// <summary>
        /// Изменение аватара автора
        /// </summary>
        Task<bool> UpdateAuthorAvatar(Guid userId, string avatarId);
    }
}