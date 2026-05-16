namespace Requests.Course.Content
{
    public record CreateCourseContentRequest(
        Guid CourseModuleId,
        string Name,
        string LinkOnRutube,
        string LinkOnVk,
        string LinkOnYoutube,
        string ImgUrl);
}