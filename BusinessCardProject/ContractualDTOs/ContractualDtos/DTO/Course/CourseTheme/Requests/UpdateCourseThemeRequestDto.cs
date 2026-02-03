namespace ContractualDtos.DTO.Course.CourseTheme.Requests;

public record UpdateCourseThemeRequestDto(
    Guid Id,
    string Name,
    string Description,
    Guid TypeOfCourseId,
    Guid ProgrammingLanguageId,
    int DisplayOrder,
    bool IsActive,
    bool IsFree);