using ByteCodePlatform.Domain.Common;
using ByteCodePlatform.Domain.Entities.Course.Theme;
using NodaTime;

namespace ByteCodePlatform.Domain.Entities
{
    public class AuthorEntity : Entity
    {
        public AuthorEntity(Instant createdAt, Instant? updatedAt, Instant? deletedAt, Guid userId)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            DeletedAt = deletedAt;
            UserId = userId;
            Courses = new HashSet<CourseThemeEntity>();
        }

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
    }
}