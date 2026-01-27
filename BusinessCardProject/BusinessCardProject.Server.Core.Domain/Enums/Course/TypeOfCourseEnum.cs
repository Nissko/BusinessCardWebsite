using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Enums.Course;

public class TypeOfCourseEnum : Enumeration
{
    public TypeOfCourseEnum(Guid id, string name) : base(id, name)
    {
    }

    public static IEnumerable<TypeOfCourseEnum> List()
    {
        return
        [
            VideoCourseType,
            TextCourseType
        ];
    }

    /// <summary>
    /// Получение типа курса по названию
    /// </summary>
    public static TypeOfCourseEnum FromName(string typeOfCourseFromName)
    {
        var request = List()
            .SingleOrDefault(s =>
                string.Equals(s.Name, typeOfCourseFromName, StringComparison.CurrentCultureIgnoreCase));

        if (request != null) return request;
        {
            var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Name));

            /*TODO: Кастомное исключение*/
            throw new ArgumentNullException(typeOfCourseIsExists);
        }
    }

    /// <summary>
    /// Получение курса по его Id
    /// </summary>
    public static string FromId(Guid fieldTypeId)
    {
        var request = List().SingleOrDefault(s => s.Id == fieldTypeId);

        if (request != null) return request.ToString();
        {
            var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Id));

            throw new Exception(typeOfCourseIsExists);
        }
    }

    /// <summary>
    /// Типы курсов
    /// </summary>
    public static readonly TypeOfCourseEnum VideoCourseType =
        new(Guid.Parse("2091ba8a-a99c-4605-929f-de6c72bf86e8"), "Видеокурс".ToLowerInvariant());

    public static readonly TypeOfCourseEnum TextCourseType =
        new(Guid.Parse("0cf77df5-a114-4bde-be62-c6c65efc6484"), "Текстовый".ToLowerInvariant());
}