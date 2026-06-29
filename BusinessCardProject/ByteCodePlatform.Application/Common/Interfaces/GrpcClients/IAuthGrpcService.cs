using Dtos.DTO.Pagination;
using Dtos.DTO.User;
using Requests.User;

namespace ByteCodePlatform.Application.Common.Interfaces.GrpcClients
{
    public interface IAuthGrpcService
    {
        /// <summary>
        /// Добавление роли "Автор"
        /// </summary>
        Task<bool> AddAuthorRole(Guid userId);

        /// <summary>
        /// Получить информацию о пользователе из сервиса аутентификации
        /// </summary>
        Task<PaginationDto<UserDto>> GetUsersInfoFromSearch(GetUsersSearchRequest request);
    }
}