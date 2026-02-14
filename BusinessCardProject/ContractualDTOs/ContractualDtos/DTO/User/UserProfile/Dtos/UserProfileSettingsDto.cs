using BusinessCardProject.Server.Core.Domain.Enums.UserSetting;

namespace ContractualDtos.DTO.User.UserProfile.Dtos
{
    public record UserProfileSettingsDto
    {
        /// <summary>
        /// Профиль пользователя у которого меняем настройки
        /// </summary>
        public Guid UserProfileId { get; init; }
    
        /// <summary>
        /// Тема
        /// </summary>
        public bool IsDark { get; init; }

        /// <summary>
        /// Открыто ли меню
        /// </summary>
        public bool IsDrawerOpen { get; init; }

        /// <summary>
        /// Платформа для видео
        /// </summary>
        public SelectPlatformEnum Platform { get; init; }
    }
}