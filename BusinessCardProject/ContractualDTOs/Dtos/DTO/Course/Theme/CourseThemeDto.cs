using Dtos.DTO.Course.ProgramLanguage;
using Dtos.DTO.User;
using NodaTime;

namespace Dtos.DTO.Course.Theme
{
    public record CourseThemeDto(
        Guid Id,
        string Name,
        string Description,
        string AvatarUrl,
        decimal Price,
        decimal? OldPrice,
        Instant CreatedAt,
        Instant? UpdatedAt,
        UserAuthorDto Author,
        ProgrammingLanguageDto ProgrammingLanguage);
}