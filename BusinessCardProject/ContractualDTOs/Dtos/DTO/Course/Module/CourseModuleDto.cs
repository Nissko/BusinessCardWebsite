using Dtos.DTO.Course.ProgramLanguage;
using NodaTime;

namespace Dtos.DTO.Course.Module
{
    public record CourseModuleDto(
        Guid Id,
        string Name,
        Instant CreatedAt,
        Instant? UpdatedAt,
        LightCourseThemeDto Theme);
}