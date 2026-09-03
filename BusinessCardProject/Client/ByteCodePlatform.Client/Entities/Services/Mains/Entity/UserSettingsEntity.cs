using BusinessCardProject.Client.Entities.Services.Mains.Options;

namespace BusinessCardProject.Client.Entities.Services.Mains.Entity
{
    internal class UserSettingsEntity
    {
        /// <summary>
        /// Состояние темы
        /// </summary>
        public bool IsDark { get; set; }

        /// <summary>
        /// Открытие меню
        /// <remarks>Можно не использовать?</remarks>
        /// </summary>
        public bool IsDrawerOpen { get; set; }

        /// <summary>
        /// Дата получения
        /// </summary>
        public DateTime? UpdateTime { get; set; } = null;

        public UserSettingsList? VideoPlatform { get; set; } = null;
    }
}