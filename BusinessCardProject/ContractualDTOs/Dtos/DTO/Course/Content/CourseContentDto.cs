using Dtos.DTO.Course.Module;
using NodaTime;

namespace Dtos.DTO.Course.Content
{
    public record CourseContentDto(
        Guid Id,
        string Name,
        string LinkRutube,
        string LinkVk,
        string LinkYoutube,
        string ImgUrl,
        Instant CreatedAt,
        Instant? UpdatedAt,
        CourseModuleDto Module);
}