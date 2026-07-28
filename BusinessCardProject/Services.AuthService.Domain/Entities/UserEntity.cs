using NodaTime;
using Services.AuthService.Domain.Common;

namespace Services.AuthService.Domain.Entities
{
    public class UserEntity : Entity
    {
        public UserEntity()
        {
            UserRoles = new HashSet<UserRolesEntity>();
            UserVerifications = new HashSet<AccountActivationEntity>();
        }
        
        public UserEntity(string surname, string name, string nickName, string email, string passwordHash,
            Instant createdAt, bool isAuthor = false, Instant? updatedAt = null, Instant? deletedAt = null)
            : this()
        {
            Surname = surname;
            Name = name;
            NickName = nickName;
            Email = email;
            PasswordHash = passwordHash;
            IsAuthor = isAuthor;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            DeletedAt = deletedAt;
            VerifyMail = false;
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; private set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Никнейм
        /// </summary>
        public string NickName { get; private set; }

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string PasswordHash { get; private set; }

        /// <summary>
        /// Является ли автором
        /// </summary>
        public bool IsAuthor { get; private set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public Instant CreatedAt { get; private set; }

        /// <summary>
        /// Даа последнего обновления
        /// </summary>
        public Instant? UpdatedAt { get; private set; }

        /// <summary>
        /// Дата удаления
        /// </summary>
        public Instant? DeletedAt { get; private set; }

        /// <summary>
        /// Активация аккаунта через подтверждение в письме
        /// </summary>
        public bool VerifyMail { get; private set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public virtual ICollection<UserRolesEntity> UserRoles { get; private set; }
        
        /// <summary>
        /// Записи активации пользователя
        /// </summary>
        public virtual ICollection<AccountActivationEntity> UserVerifications { get; private set; }
        
        /// <summary>
        /// Обновление информации пользователя
        /// </summary>
        public void UpdateUser(string? surname = null, string? name = null, string? nickName = null,
            string? email = null, Instant? updatedAt = null)
        {
            Surname = surname ?? Surname;
            Name = name ?? Name;
            NickName = nickName ?? NickName;
            Email = email ?? Email;
            UpdatedAt = updatedAt ?? SetUpdatedAt();
        }

        /// <summary>
        /// Выдача авторских прав
        /// </summary>
        public void SetAuthor(bool isAuthor)
        {
            IsAuthor = isAuthor;
            UpdatedAt = SetUpdatedAt();
        }
        
        public void SetVerified()
        {
            VerifyMail = true;
            UpdatedAt = SetUpdatedAt();
        }
        
        private static Instant? SetUpdatedAt()
        {
            return SystemClock.Instance.GetCurrentInstant();
        }
    }
}