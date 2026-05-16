using ByteCodePlatform.Domain.Common;
using NodaTime;

namespace ByteCodePlatform.Domain.Entities
{
    public class UserEntity(
        string surname,
        string name,
        string nickName,
        string email,
        Instant createdAt,
        bool isAuthor = false,
        Instant? updatedAt = null,
        Instant? deletedAt = null)
        : Entity
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; private set; } = surname;

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; private set; } = name;

        /// <summary>
        /// Никнейм
        /// </summary>
        public string NickName { get; private set; } = nickName;

        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; private set; } = email;

        /// <summary>
        /// Является ли автором
        /// </summary>
        public bool IsAuthor {  get; private set; } = isAuthor;
        
        /// <summary>
        /// Дата создания
        /// </summary>
        public Instant CreatedAt { get; private set; } = createdAt;

        /// <summary>
        /// Даа последнего обновления
        /// </summary>
        public Instant? UpdatedAt { get; private set; } = updatedAt;

        /// <summary>
        /// Дата удаления
        /// </summary>
        public Instant? DeletedAt { get; private set; } = deletedAt;

        public virtual AuthorEntity Author { get; private set; }

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
            UpdatedAt = updatedAt ?? UpdatedAt;
        }
        
        /// <summary>
        /// Выдача авторских прав
        /// </summary>
        public void SetAuthor(bool isAuthor)
        {
            IsAuthor = isAuthor;
            UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        }
    }
}