namespace Requests.Course.Module
{
    public record CreateCourseModuleRequest(Guid CourseThemeId, string Name);
}