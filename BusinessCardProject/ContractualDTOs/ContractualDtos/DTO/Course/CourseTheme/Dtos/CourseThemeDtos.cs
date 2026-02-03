using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;

namespace ContractualDtos.DTO.Course.CourseTheme.Dtos;

/// <summary>
/// DTO для возврата объекта
/// </summary>
public record CourseThemeDtos(
    Guid Id,
    string Name,
    string Description,
    Guid TypeOfCourseId,
    ProgrammingLanguageDtos ProgrammingLanguage,
    int DisplayOrder,
    bool IsActive,
    bool IsFree);