namespace RequestModels.Course.Module
{
    public record UpdateCourseModuleRequest(
        Guid Id,
        Guid? CourseThemeId,
        string? Name,
        bool? IsShow,
        int? DisplayOrder);
}