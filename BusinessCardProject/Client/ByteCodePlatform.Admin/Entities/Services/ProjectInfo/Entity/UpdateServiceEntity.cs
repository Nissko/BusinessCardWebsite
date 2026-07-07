using ByteCodePlatform.Admin.Entities.Services.ProjectInfo.Interfaces;
using ByteCodePlatform.Admin.Features.admin.dynamic_edit_dialog;
using CourseService.Proto;
using Google.Protobuf;

namespace ByteCodePlatform.Admin.Entities.Services.ProjectInfo.Entity
{
    public class EntityUpdateService : IEntityUpdateService
    {
        private readonly CourseService.Proto.CourseService.CourseServiceClient _courseService;

        public EntityUpdateService(CourseService.Proto.CourseService.CourseServiceClient courseService)
        {
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
        }

        public async Task<bool> UpdateFieldAsync(TypeOfEntityType entityType, Guid id, string fieldName, object? value)
        {
            return entityType switch
            {
                TypeOfEntityType.CourseThemes => await UpdateCourseThemeAsync(id, fieldName, value),
                TypeOfEntityType.CourseModule => await UpdateCourseModuleAsync(id, fieldName, value),
                TypeOfEntityType.VideoCourse => await UpdateCourseVideoAsync(id, fieldName, value),
                _ => throw new ArgumentOutOfRangeException(nameof(entityType))
            };
        }

        private async Task<bool> UpdateCourseThemeAsync(Guid id, string fieldName, object? value)
        {
            var request = new UpdateCourseThemeRequest { Id = id.ToString() };
            SetOptionalField(request, fieldName, value);
            var response = await _courseService.UpdateCourseThemeAsync(request);
            return response.Success;
        }

        private async Task<bool> UpdateCourseModuleAsync(Guid id, string fieldName, object? value)
        {
            var request = new UpdateCourseModuleRequest { Id = id.ToString() };
            SetOptionalField(request, fieldName, value);
            var response = await _courseService.UpdateCourseModuleAsync(request);
            return response.Success;
        }

        private async Task<bool> UpdateCourseVideoAsync(Guid id, string fieldName, object? value)
        {
            var request = new UpdateCourseContentRequest { Id = id.ToString() };
            SetOptionalField(request, fieldName, value);
            var response = await _courseService.UpdateCourseContentAsync(request);
            return response.Success;
        }

        /// <summary>
        /// Метод установки необязательного поля через рефлексию.
        /// </summary>
        private static void SetOptionalField(IMessage message, string fieldName, object? value)
        {
            if (value == null) return;

            var descriptor = message.Descriptor;
            var fieldDescriptor = descriptor.FindFieldByName(ToSnakeCase(fieldName));

            if (fieldDescriptor == null)
            {
                throw new ArgumentException($"Поле '{fieldName}' не найдено в {descriptor.Name}");
            }

            var propertyName = ToPascalCase(fieldName);
            var property = message.GetType().GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException($"Свойство '{propertyName}' не найдено в {message.GetType().Name}");
            }

            var targetType = property.PropertyType;
            var convertedValue = ConvertValue(value, targetType);

            property.SetValue(message, convertedValue);
        }

        /// <summary>
        /// Конвертация значения к целевому типу свойства gRPC-сообщения
        /// </summary>
        private static object? ConvertValue(object value, Type targetType)
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (targetType == typeof(string))
            {
                return value.ToString();
            }

            if (underlyingType == typeof(int) || underlyingType == typeof(int?))
            {
                return Convert.ToInt32(value);
            }

            if (underlyingType == typeof(double) || underlyingType == typeof(double?))
            {
                return Convert.ToDouble(value);
            }

            if (underlyingType == typeof(bool) || underlyingType == typeof(bool?))
            {
                if (value is string strValue)
                    return bool.Parse(strValue);
                return Convert.ToBoolean(value);
            }

            return Convert.ChangeType(value, underlyingType);
        }
        
        private static string ToSnakeCase(string name)
        {
            return string.Concat(name.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())).ToLower();
        }
        
        private static string ToPascalCase(string name)
        {
            if (!name.Contains('_') && char.IsUpper(name[0]))
            {
                return name;
            }

            return string.Concat(name.Split('_').Select(part =>
                char.ToUpper(part[0]) + part.Substring(1).ToLower()));
        }
    }
}