using BusinessCardProject.Server.Core.Domain.Commons;
using BusinessCardProject.Server.Core.Domain.Enums.Course;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course;

/// <summary>
/// Название блока курса (категория)
/// <example>База, ООП и тд</example>
/// </summary>
internal class CourseThemeEntity : Entity
{
    public CourseThemeEntity(string themeName, string themeDescription, TypeOfCourseEnum typeOfCourse,
        Guid programmingLanguageId)
    {
        _themeName = themeName;
        _typeOfCourse = typeOfCourse;
        _programmingLanguageId = programmingLanguageId;
        _themeDescription = themeDescription;
    }

    /// <summary>
    /// Название темы курса
    /// </summary>
    private string _themeName;

    /// <summary>
    /// Описание темы курса
    /// </summary>
    private string _themeDescription;

    /// <summary>
    /// Тип курса
    /// </summary>
    private TypeOfCourseEnum _typeOfCourse;

    /// <summary>
    /// ЯП к которому принадлежит тема
    /// </summary>
    private Guid _programmingLanguageId;
}