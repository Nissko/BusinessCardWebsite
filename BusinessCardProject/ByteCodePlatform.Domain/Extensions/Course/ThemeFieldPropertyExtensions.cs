using ByteCodePlatform.Domain.Entities.Course.Theme;
using ByteCodePlatform.Domain.Enums;

namespace ByteCodePlatform.Domain.Extensions.Course
{
    public static class ThemeFieldPropertyExtensions
    {
        private const string DefaultIsShow = "false";
        private const string DefaultDisplayOrder = "10";
        
        /// <summary>
        /// Получение свойства по типу
        /// </summary>
        public static CourseThemeFieldPropertyEntity? GetProperty(this IEnumerable<CourseThemeFieldPropertyEntity> properties, Guid type)
        {
            return properties.FirstOrDefault(x => x.FieldPropertyType.Id == type);
        }
        
        /// <summary>
        /// Добавление свойств по умолчанию
        /// </summary>
        public static void SetDefault(this ICollection<CourseThemeFieldPropertyEntity> properties, Guid themeId)
        {
            properties.Add(new(DefaultIsShow, FieldPropertyTypesEnum.IsShow, themeId));
            properties.Add(new(DefaultDisplayOrder, FieldPropertyTypesEnum.DisplayOrder, themeId));
        }
    
        /// <summary>
        /// Проверка наличия свойств
        /// </summary>
        public static bool CheckProperties(this IEnumerable<CourseThemeFieldPropertyEntity> properties)
        {
            var courseThemeFieldProperties = properties.ToList();
            if (!courseThemeFieldProperties.Any()) throw new("Properties cannot be empty");

            if (courseThemeFieldProperties.All(x => x.FieldPropertyTypeId != FieldPropertyTypesEnum.DisplayOrder))
                throw new(
                    $"Required property '{FieldPropertyTypesEnum.DisplayOrder}' not found");

            if (courseThemeFieldProperties.All(x => x.FieldPropertyTypeId != FieldPropertyTypesEnum.IsShow))
                throw new(
                    $"Required property '{FieldPropertyTypesEnum.IsShow}' not found");
            
            return true;
        }
    }
}