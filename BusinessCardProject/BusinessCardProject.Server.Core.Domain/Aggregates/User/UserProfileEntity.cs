using BusinessCardProject.Server.Core.Domain.Aggregates.User.Setting;
using BusinessCardProject.Server.Core.Domain.Commons;
using BusinessCardProject.Server.Core.Domain.Enums.User;
using BusinessCardProject.Server.Core.Domain.Enums.UserSetting;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.User
{
    /// <summary>
    /// Профиль пользователя
    /// </summary>
    internal class UserProfileEntity : Entity
    {
        public UserProfileEntity(string surname, string name, string patronymic,
            string email, string password)
        {
            _surname = surname;
            _name = name;
            _patronymic = patronymic;
            _email = email;
            _password = password;
            _dateOfRegistered = DateTime.Now;
            _userRole = UserRoleEnum.User;

            /*TODO: Потом исправить и сделать чтобы было True после подтверждения*/
            _isActive = true;
            _isBlocked = false;
        }

        #region Public Fields

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname => _surname;

        /// <summary>
        /// Имя
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic => _patronymic;

        /// <summary>
        /// Почта
        /// </summary>
        public string Email => _email;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password => _password;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public Guid UserRole => _userRole.Id;

        /// <summary>
        /// Дата и время регистрации
        /// </summary>
        public DateTime DateOfRegistered => _dateOfRegistered;

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

        #region Private Fields

        /// <summary>
        /// Фамилия
        /// </summary>
        private string _surname;

        /// <summary>
        /// Имя
        /// </summary>
        private string _name;

        /// <summary>
        /// Фамилия
        /// </summary>
        private string _patronymic;

        /// <summary>
        /// Почта
        /// </summary>
        private string _email;

        /// <summary>
        /// Пароль
        /// </summary>
        private string _password;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        private UserRoleEnum _userRole;

        /// <summary>
        /// Дата и время регистрации
        /// </summary>
        private DateTime _dateOfRegistered;

        #endregion

        #region Options

        /// <summary>
        /// Признак активности.
        /// Нужен для того, чтобы аккаунт работал после подтверждения через почту.
        /// </summary>
        private bool _isActive;

        /// <summary>
        /// Заблокирован пользователь или нет
        /// </summary>
        private bool _isBlocked;

        /// <summary>
        /// Настройки пользователя
        /// </summary>
        private UserSetting _setting = new()
        {
            [SettingKeys.IsDark] = false,
            [SettingKeys.IsDrawerOpen] = false,
            [SettingKeys.UpdateTime] = $"{DateTime.Now}",
            [SettingKeys.Platform] = (int)SelectPlatformEnum.YouTube
        };

        #endregion
    }
}