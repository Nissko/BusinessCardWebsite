using BusinessCardProject.Server.Core.Domain.Aggregates.User;
using ContractualDtos.DTO.Table;
using ContractualDtos.DTO.User.UserProfile.Dtos;
using ContractualDtos.DTO.User.UserProfile.Requests;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.User
{
    public interface IUserRepository
    {
        /// <summary>
        /// Вывод всех пользователей
        /// </summary>
        Task<TableResponse<UserProfileDtos>> GetAllAsync(
            int page,
            int pageSize,
            string? search = null,
            string? sortBy = null,
            string? sortDirection = "Ascending");

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<bool?> Create(CreateUserProfileRequestDto dto);

        /// <summary>
        /// Поиск определенного пользователя
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<UserProfileDtos?> Read(Guid id);
    
        /// <summary>
        /// Поиск определенного пользователя
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<UserProfileEntity> GetUserEntityFromId(Guid id);

        /// <summary>
        /// Изменение определенного пользователя
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<UserProfileDtos?> Update(UpdateUserProfileRequestDto dto);

        /// <summary>
        /// Деактивация определенного пользователя
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<bool> SafeDelete(Guid id);
    
        /// <summary>
        /// Восстановление профиля пользователя, если спустя время он решил восстановить удаленный аккаунт
        /// </summary>
        Task<bool> RecoveryUserProfile(Guid id);

        /// <summary>
        /// Добавление новой роли для пользователя
        /// </summary>
        Task<bool> AddNewRole(AddNewUserRoleRequestDto dto);

        /// <summary>
        /// Изменение настроек пользователя
        /// </summary>
        Task<bool> UpdateUserSettings(UserProfileSettingsDto settings);

        /// <summary>
        /// Авторизация пользоватея
        /// </summary>
        /// TODO: возвращать AccessToken
        Task<bool> Login(AuthUserDto dto);
    }
}