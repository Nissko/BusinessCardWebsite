using ByteCodePlatform.Admin.Entities.Services.ProjectInfo.Options;

namespace ByteCodePlatform.Admin.Entities.Services.ProjectInfo.Entity
{
    internal class UserSettingsEntity
    {
        /// <summary>
        /// Состояние темы
        /// </summary>
        public bool IsDark { get; set; }

        /// <summary>
        /// Открытие меню
        /// <remarks>Можно не использовать?</remarks>F
        /// </summary>
        public bool IsDrawerOpen { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public UserSettingsList? VideoPlatform { get; set; } = null;
        
        /// <summary>
        /// 
        /// </summary>
        public UserLanguagesList? ProgrammingLanguage { get; set; } = null;
    }
}