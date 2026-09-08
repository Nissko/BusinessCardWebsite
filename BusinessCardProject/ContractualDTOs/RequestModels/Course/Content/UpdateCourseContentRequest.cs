namespace RequestModels.Course.Content
{
    public record UpdateCourseContentRequest(
        Guid Id,
        Guid? CourseModuleId,
        string? Name,
        string? LinkOnRutube,
        string? LinkOnVk,
        string? LinkOnYoutube,
        string? ImgUrl,
        bool? IsShow,
        int? DisplayOrder);
}