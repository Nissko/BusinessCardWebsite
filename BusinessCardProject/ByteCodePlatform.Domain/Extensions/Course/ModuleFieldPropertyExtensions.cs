using ByteCodePlatform.Domain.Entities.Course.Module;
using ByteCodePlatform.Domain.Enums;

namespace ByteCodePlatform.Domain.Extensions.Course
{
    /// <summary>
    /// TODO: перепроверить код, т.к копировался с тем.
    /// </summary>
    public static class ModuleFieldPropertyExtensions
    {
        private const string DefaultIsShow = "false";
        private const string DefaultDisplayOrder = "10";
        
        /// <summary>
        /// Получение свойства по типу
        /// </summary>
        public static CourseModuleFieldPropertyEntity? GetProperty(this IEnumerable<CourseModuleFieldPropertyEntity> properties, Guid type)
        {
            return properties.FirstOrDefault(x => x.FieldPropertyType.Id == type);
        }
        
        /// <summary>
        /// Добавление свойств по умолчанию
        /// </summary>
        public static void SetDefault(this ICollection<CourseModuleFieldPropertyEntity> properties, Guid moduleId)
        {
            properties.Add(new(DefaultIsShow, FieldPropertyTypesEnum.IsShow, moduleId));
            properties.Add(new(DefaultDisplayOrder, FieldPropertyTypesEnum.DisplayOrder, moduleId));
        }
    
        /// <summary>
        /// Проверка наличия свойств
        /// </summary>
        public static bool CheckProperties(this IEnumerable<CourseModuleFieldPropertyEntity> courseModuleFieldProperties)
        {
            var properties = courseModuleFieldProperties.ToList();
            if (!properties.Any()) throw new("Properties cannot be empty");

            if (properties.All(x => x.FieldPropertyTypeId != FieldPropertyTypesEnum.DisplayOrder))
                throw new($"Required property '{FieldPropertyTypesEnum.DisplayOrder}' not found");

            if (properties.All(x => x.FieldPropertyTypeId != FieldPropertyTypesEnum.IsShow))
                throw new($"Required property '{FieldPropertyTypesEnum.IsShow}' not found");
            
            return true;
        }
    }
}