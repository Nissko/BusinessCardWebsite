using ByteCodePlatform.Domain.Common;

namespace ByteCodePlatform.Domain.Entities.Course.Theme
{
    public class CourseThemeFieldPropertyEntity : Entity
    {
        public CourseThemeFieldPropertyEntity(string value, Guid fieldPropertyTypeId, Guid courseThemeId)
        {
            Value = value;
            FieldPropertyTypeId = fieldPropertyTypeId;
            CourseThemeId = courseThemeId;
        }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Свойство, которое нужно добавить на поле
        /// </summary>
        public Guid FieldPropertyTypeId { get; private set; }

        public virtual FieldPropertyTypeEntity FieldPropertyType { get; private set; }

        /// <summary>
        /// Тема курса, на который нужно добавить
        /// </summary>
        public Guid CourseThemeId { get; private set; }

        public virtual CourseThemeEntity CourseTheme { get; private set; }

        public void UpdateValue(string? value)
        {
            Value = value ?? Value;
        }
    }
}