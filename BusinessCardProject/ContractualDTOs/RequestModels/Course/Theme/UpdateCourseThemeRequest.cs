namespace RequestModels.Course.Theme
{
    public record UpdateCourseThemeRequest(
        Guid Id,
        Guid? ProgrammingLanguageId,
        string? Name,
        string? Description,
        string? AvatarUrl,
        double? Price,
        double? OldPrice,
        bool? IsShow,
        int? DisplayOrder,
        bool? IsFree,
        bool? IsDiscount);
}