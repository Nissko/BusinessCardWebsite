using BusinessCardProject.Server.Core.Domain.Aggregates.Course.Abstracts;
using BusinessCardProject.Server.Core.Domain.Aggregates.User.Setting;
using BusinessCardProject.Server.Core.Domain.Enums.UserSetting;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.User
{
    /// <summary>
    /// Профиль пользователя
    /// </summary>
    public class UserProfileEntity(
        string surname,
        string name,
        string patronymic,
        string email,
        string altName,
        string password)
        : PersonInfoAbstract(surname, name, patronymic, email, altName, DateTime.Now, null)
    {
        /*TODO: Потом исправить и сделать чтобы было True после подтверждения*/

        #region Public Fields

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password => _password;

        /// <summary>
        /// Активен ли пользователь
        /// </summary>
        public bool IsActive => _isActive;

        /// <summary>
        /// Заблокирован или нет
        /// </summary>
        public bool IsBlocked => _isBlocked;

        /// <summary>
        /// Настройки пользователя
        /// </summary>
        public UserSetting Settings
        {
            get => _setting;
            set => _setting = value;
        }

        #endregion

        #region Private properties

        /// <summary>
        /// Пароль
        /// </summary>
        private string _password = password;

        #endregion

        #region virtual

        /// <summary>
        /// Коллекция категорий подготовок
        /// </summary>
        public virtual ICollection<UserRoleEntity> UserRoles { get; private set; } = new HashSet<UserRoleEntity>();

        #endregion

        #region options

        /// <summary>
        /// Признак активности.
        /// Нужен для того, чтобы аккаунт работал после подтверждения через почту.
        /// </summary>
        private bool _isActive = false;

        /// <summary>
        /// Заблокирован пользователь или нет
        /// </summary>
        private bool _isBlocked = false;

        /// <summary>
        /// Настройки пользователя
        /// </summary>
        private UserSetting _setting = new()
        {
            [SettingKeys.IsDark] = false,
            [SettingKeys.IsDrawerOpen] = false,
            [SettingKeys.UpdateTime] = DateTime.UtcNow.ToString("u"),
            [SettingKeys.Platform] = (int)SelectPlatformEnum.YouTube
        };

        #endregion

        #region functions

        /// <summary>
        /// Функционал для добавления роли
        /// </summary>
        public void AddRole(UserRoleEntity userRole)
        {
            UserRoles.Add(userRole);
        }

        #endregion
    }
}