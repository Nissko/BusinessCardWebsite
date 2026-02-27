namespace ContractualDtos.DTO.Course.CourseModule.Requests
{
    /// <summary>
    /// DTO для запросов
    /// </summary>
    public record CreateCourseModuleRequestDto(
        string Name,
        string Description,
        int DisplayOrder,
        bool IsShow,
        Guid CourseThemeId);
}