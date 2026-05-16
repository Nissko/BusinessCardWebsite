using ByteCodePlatform.Domain.Common;
using ByteCodePlatform.Domain.Entities.Course.Content;
using ByteCodePlatform.Domain.Entities.Course.Theme;
using NodaTime;

namespace ByteCodePlatform.Domain.Entities.Course.Module
{
    public class CourseModuleEntity : Entity
    {
        public CourseModuleEntity(string name, Guid courseThemeId, 
            Instant createdAt, Instant? updatedAt = null)
        {
            Name = name;
            CourseThemeId = courseThemeId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Contents = new HashSet<CourseContentEntity>();
            ModuleFieldProperties = new HashSet<CourseModuleFieldPropertyEntity>();
        }

        /// <summary>
        /// Название модуля
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public Instant CreatedAt { get; private set; }

        /// <summary>
        /// Дата редактирования
        /// </summary>
        public Instant? UpdatedAt { get; private set; }

        /// <summary>
        /// Тема к которой относится модуль
        /// </summary>
        public virtual CourseThemeEntity CourseTheme { get; private set; }

        public Guid CourseThemeId { get; private set; }

        /// <summary>
        /// Содержание модуля
        /// </summary>
        public virtual ICollection<CourseContentEntity> Contents { get; private set; }
        
        public virtual ICollection<CourseModuleFieldPropertyEntity> ModuleFieldProperties { get; private set; }

        public void Update(string? name, Guid? courseThemeId)
        {
            Name = string.IsNullOrEmpty(name) ? Name : name;
            CourseThemeId = courseThemeId ?? CourseThemeId;
            UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        }
    }
}