using ContractualDtos.DTO.Course.CourseTheme.Dtos;

namespace ContractualDtos.DTO.Course.CourseModule.Dtos;

/// <summary>
/// DTO для возврата объекта
/// </summary>
public record CourseModuleDtos(
    Guid Id,
    string Name,
    string Description,
    CourseThemeDtos CourseTheme)
{
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; init; } = Name;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// Тема курса
    /// </summary>
    public CourseThemeDtos CourseTheme { get; init; } = CourseTheme;
}