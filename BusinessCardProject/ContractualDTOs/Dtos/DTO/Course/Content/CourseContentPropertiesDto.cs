namespace Dtos.DTO.Course.Content
{
    public record CourseContentPropertiesDto(
        Guid CourseContentId,
        int DisplayOrder,
        bool IsShow);
}