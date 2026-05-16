namespace Requests.Course.Theme
{
    public record UpdateCourseThemeRequest(
        Guid Id,
        Guid? ProgrammingLanguageId,
        string? Name,
        string? Description,
        string? AvatarUrl,
        decimal? Price,
        decimal? OldPrice,
        bool? IsShow,
        int? DisplayOrder);
}