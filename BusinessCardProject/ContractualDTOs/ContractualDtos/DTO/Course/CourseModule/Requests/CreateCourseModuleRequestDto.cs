namespace ContractualDtos.DTO.Course.CourseModule.Requests
{
    /// <summary>
    /// DTO для запросов
    /// </summary>
    public record CreateCourseModuleRequestDto(
        string Name,
        string Description,
        Guid CourseThemeId);
}