using Dtos.DTO.Course.Theme;
using NodaTime;

namespace Dtos.DTO.Course.Module
{
    public record CourseModuleDto(
        Guid Id,
        string Name,
        Instant CreatedAt,
        Instant? UpdatedAt,
        CourseThemeDto Theme);
}