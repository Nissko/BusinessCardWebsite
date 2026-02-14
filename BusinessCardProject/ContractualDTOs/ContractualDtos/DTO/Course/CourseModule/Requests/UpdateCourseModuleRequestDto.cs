namespace ContractualDtos.DTO.Course.CourseModule.Requests
{
    public record UpdateCourseModuleRequestDto(
        Guid Id,
        string Name,
        string Description,
        Guid CourseThemeId);
}