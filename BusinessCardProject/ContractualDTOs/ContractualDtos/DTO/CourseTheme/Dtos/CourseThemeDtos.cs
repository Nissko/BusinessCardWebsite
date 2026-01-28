namespace ContractualDtos.DTO.CourseTheme.Dtos;

/// <summary>
/// DTO для возврата объекта
/// </summary>
public record CourseThemeDtos(
    Guid Id,
    string Name,
    string Description,
    Guid TypeOfCourseId,
    Guid ProgrammingLanguageId)
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// Название темы
    /// </summary>
    public string Name { get; init; } = Name;

    /// <summary>
    /// Название темы
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// Идентификатор типа курса
    /// </summary>
    public Guid TypeOfCourseId { get; init; } = TypeOfCourseId;

    /// <summary>
    /// Идентификатор ЯП которому принадлежит тема
    /// </summary>
    public Guid ProgrammingLanguageId { get; init; } = ProgrammingLanguageId;
}