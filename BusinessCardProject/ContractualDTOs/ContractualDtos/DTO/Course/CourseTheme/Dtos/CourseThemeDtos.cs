using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;

namespace ContractualDtos.DTO.Course.CourseTheme.Dtos
{
    public record CourseThemeDtos(
        Guid Id,
        string Name,
        string Description,
        Guid TypeOfCourseId,
        ProgrammingLanguageDtos ProgrammingLanguage,
        List<CourseThemeRecommendationDtos> CourseThemeRecommendation,
        int DisplayOrder,
        bool IsActive,
        bool IsFree);
}