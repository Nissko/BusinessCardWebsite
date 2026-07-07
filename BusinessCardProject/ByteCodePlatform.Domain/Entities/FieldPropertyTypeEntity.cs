using ByteCodePlatform.Domain.Common;
using ByteCodePlatform.Domain.Entities.Course.Content;
using ByteCodePlatform.Domain.Entities.Course.Module;
using ByteCodePlatform.Domain.Entities.Course.Theme;

namespace ByteCodePlatform.Domain.Entities
{
    public class FieldPropertyTypeEntity : Entity
    {
        public FieldPropertyTypeEntity(string name, string description)
        {
            Name = name;
            Description = description;
            CourseContent = new HashSet<CourseContentFieldPropertyEntity>();
            CourseModule = new HashSet<CourseModuleFieldPropertyEntity>();
            CourseTheme = new HashSet<CourseThemeFieldPropertyEntity>();
        }

        /// <summary>
        /// Наименование свойства
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Описание свойства
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Свойства на контенте
        /// </summary>
        public virtual ICollection<CourseContentFieldPropertyEntity> CourseContent { get; private set; }

        /// <summary>
        /// Свойства на модулях
        /// </summary>
        public virtual ICollection<CourseModuleFieldPropertyEntity> CourseModule { get; private set; }

        /// <summary>
        /// Свойства на темах
        /// </summary>
        public virtual ICollection<CourseThemeFieldPropertyEntity> CourseTheme { get; private set; }
    }
}