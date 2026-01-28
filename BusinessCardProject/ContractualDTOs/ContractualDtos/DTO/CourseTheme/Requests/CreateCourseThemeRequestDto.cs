namespace ContractualDtos.DTO.CourseTheme.Requests;

/// <summary>
/// DTO для запросов
/// </summary>
public record CreateCourseThemeRequestDto(
    string Name,
    string Description,
    Guid TypeOfCourseId,
    Guid ProgrammingLanguageId);