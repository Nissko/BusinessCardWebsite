namespace Dtos.DTO.Course.Theme
{
    public record CourseThemePropertiesDto(
        Guid CourseThemeId, 
        int DisplayOrder,
        bool IsShow,
        bool IsFree);
}