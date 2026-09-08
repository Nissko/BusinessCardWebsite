using DTOs.DTO.Course.ProgramLanguage;
using NodaTime;

namespace DTOs.DTO.Course.Module
{
    public record CourseModuleDto(
        Guid Id,
        string Name,
        Instant CreatedAt,
        Instant? UpdatedAt,
        LightCourseThemeDto Theme);
}