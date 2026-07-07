namespace Requests.Course.Theme
{
    public record CreateCourseThemeRequest(
        Guid ProgrammingLanguageId,
        Guid AuthorId,
        string Name,
        string Description,
        string AvatarUrl,
        double Price,
        double? OldPrice);
}