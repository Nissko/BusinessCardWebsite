using ByteCodePlatform.Domain.Common;

namespace ByteCodePlatform.Domain.Entities.Course.Content
{
    public class CourseContentFieldPropertyEntity : Entity
    {
        public CourseContentFieldPropertyEntity(string value, Guid fieldPropertyTypeId, Guid courseContentId)
        {
            Value = value;
            FieldPropertyTypeId = fieldPropertyTypeId;
            CourseContentId = courseContentId;
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
        /// Контент курса, на который нужно добавить
        /// </summary>
        public Guid CourseContentId { get; private set; }
        public virtual CourseContentEntity CourseContent { get; private set; }
        
        public void UpdateValue(string? value)
        {
            Value = value ?? Value;
        }
    }
}