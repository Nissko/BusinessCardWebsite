using ByteCodePlatform.Domain.Common;

namespace ByteCodePlatform.Domain.Entities.Course.Module
{
    public class CourseModuleFieldPropertyEntity : Entity
    {
        public CourseModuleFieldPropertyEntity(string value, Guid fieldPropertyTypeId, Guid courseModuleId)
        {
            Value = value;
            FieldPropertyTypeId = fieldPropertyTypeId;
            CourseModuleId = courseModuleId;
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
        /// Модуль курса, на который нужно добавить
        /// </summary>
        public Guid CourseModuleId { get; private set; }
        public virtual CourseModuleEntity CourseModule { get; private set; }
        
        public void UpdateValue(string? value)
        {
            Value = value ?? Value;
        }
    }
}