using BusinessCardProject.Client.Entities.Services.ProjectInfo.Options;

namespace BusinessCardProject.Client.Entities.Services.ProjectInfo.Entity
{
    internal class UserSettingsEntity
    {
        /// <summary>
        /// Состояние темы
        /// </summary>
        public bool IsDark { get; set; }

        /// <summary>
        /// Открытие меню
        /// TODO:Пересмотреть реализацию
        /// <remarks>Можно не использовать?</remarks>
        /// </summary>
        public bool IsDrawerOpen { get; set; }

        /// <summary>
        /// Дата получения
        /// TODO: Сделать реализацию. Что если Null, то должны делать Load(), а затем заполнять UpdateTime
        /// </summary>
        public DateTime? UpdateTime { get; set; } = null;

        public UserSettingsList? VideoPlatform { get; set; } = null;
    }
}