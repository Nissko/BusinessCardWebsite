using ByteCodePlatform.Domain.Common;
using ByteCodePlatform.Domain.Entities.Course.Theme;

namespace ByteCodePlatform.Domain.Entities.Course
{
    public class ProgrammingLanguageCategoryEntity : Entity
    {
        public ProgrammingLanguageCategoryEntity(string name)
        {
            Name = name;
            CoursesThemes = new HashSet<CourseThemeEntity>();
        }

        /// <summary>
        /// Название ЯП
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Темы курсов которые есть у ЯП
        /// </summary>
        public virtual ICollection<CourseThemeEntity> CoursesThemes { get; private set; }
    }
}