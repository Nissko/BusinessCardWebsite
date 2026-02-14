namespace ContractualDtos.DTO.Course.CourseTheme.Requests
{
    /// <summary>
    /// DTO для запросов
    /// </summary>
    public record CreateCourseThemeRequestDto(
        string Name,
        string Description,
        Guid TypeOfCourseId,
        Guid ProgrammingLanguageId,
        int DisplayOrder,
        bool IsActive,
        bool IsFree);
}