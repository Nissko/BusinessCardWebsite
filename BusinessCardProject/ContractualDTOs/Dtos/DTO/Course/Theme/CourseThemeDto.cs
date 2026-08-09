using Dtos.DTO.User;
using NodaTime;

namespace Dtos.DTO.Course.Theme
{
    public record CourseThemeDto(
        Guid Id,
        string Name,
        string Description,
        string AvatarUrl,
        bool IsFree,
        int CountLessons,
        double Price,
        double? OldPrice,
        Instant CreatedAt,
        Instant? UpdatedAt,
        UserAuthorDto Author,
        LightProgrammingLanguageDto ProgrammingLanguage);
}