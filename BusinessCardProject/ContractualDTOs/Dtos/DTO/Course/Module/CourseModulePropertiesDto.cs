namespace DTOs.DTO.Course.Module
{
    public record CourseModulePropertiesDto(
        Guid CourseModuleId,
        int DisplayOrder,
        bool IsShow);
}