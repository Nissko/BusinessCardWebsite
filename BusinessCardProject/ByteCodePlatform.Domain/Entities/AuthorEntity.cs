using ByteCodePlatform.Domain.Common;
using ByteCodePlatform.Domain.Entities.Course.Theme;
using NodaTime;

namespace ByteCodePlatform.Domain.Entities
{
    public class AuthorEntity : Entity
    {
        public AuthorEntity(string name, string surname, string aboutUs, string avatarId, Instant createdAt,
            Instant? updatedAt, Instant? deletedAt, Guid userId)
        {
            Name = name;
            Surname = surname;
            AboutUs = aboutUs;
            AvatarId = avatarId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            DeletedAt = deletedAt;
            UserId = userId;
            Courses = new HashSet<CourseThemeEntity>();
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; private set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string AboutUs { get; private set; }

        /// <summary>
        /// Идентификатор аватара
        /// </summary>
        public string AvatarId { get; private set; }

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
        /// Id пользователя
        /// </summary>
        public Guid UserId { get; private set; }

        public virtual UserEntity User { get; private set; }

        /// <summary>
        /// Курсы автора
        /// </summary>
        public virtual ICollection<CourseThemeEntity> Courses { get; private set; }
        
        /// <summary>
        /// Изменение аватара пользователя
        /// </summary>
        public void UpdateAuthorAvatar(string avatarId)
        {
            AvatarId = avatarId;
        }
    }
}