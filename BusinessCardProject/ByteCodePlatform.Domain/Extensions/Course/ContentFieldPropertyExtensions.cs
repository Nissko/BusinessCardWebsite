using ByteCodePlatform.Domain.Entities.Course.Content;
using ByteCodePlatform.Domain.Enums;

namespace ByteCodePlatform.Domain.Extensions.Course
{
    /// <summary>
    /// TODO: перепроверить код, т.к копировался
    /// </summary>
    public static class ContentFieldPropertyExtensions
    {
        private const string DefaultIsShow = "false";
        private const string DefaultDisplayOrder = "10";
        
        /// <summary>
        /// Получение свойства по типу
        /// </summary>
        public static CourseContentFieldPropertyEntity? GetProperty(this IEnumerable<CourseContentFieldPropertyEntity> properties, Guid type)
        {
            return properties.FirstOrDefault(x => x.FieldPropertyType.Id == type);
        }
        
        /// <summary>
        /// Добавление свойств по умолчанию
        /// </summary>
        public static void SetDefault(this ICollection<CourseContentFieldPropertyEntity> properties, Guid moduleId)
        {
            properties.Add(new(DefaultIsShow, FieldPropertyTypesEnum.IsShow, moduleId));
            properties.Add(new(DefaultDisplayOrder, FieldPropertyTypesEnum.DisplayOrder, moduleId));
        }
    
        /// <summary>
        /// Проверка наличия свойств
        /// </summary>
        public static bool CheckProperties(this IEnumerable<CourseContentFieldPropertyEntity> properties)
        {
            var courseContentFieldProperties = properties.ToList();
            if (!courseContentFieldProperties.Any()) throw new("Свойства не могут быть пустыми");

            if (courseContentFieldProperties.All(x => x.FieldPropertyTypeId != FieldPropertyTypesEnum.DisplayOrder))
                throw new(
                    $"Требуемое свойство '{FieldPropertyTypesEnum.DisplayOrder}' не найдено");

            if (courseContentFieldProperties.All(x => x.FieldPropertyTypeId != FieldPropertyTypesEnum.IsShow))
                throw new(
                    $"Требуемое свойство '{FieldPropertyTypesEnum.IsShow}' не найдено");
            
            return true;
        }
    }
}