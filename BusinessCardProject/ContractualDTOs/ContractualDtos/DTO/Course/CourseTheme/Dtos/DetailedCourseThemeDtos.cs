using ContractualDtos.DTO.Course.CourseModule.Dtos;
using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;

namespace ContractualDtos.DTO.Course.CourseTheme.Dtos
{
    /// <summary>
    /// DTO для возврата объекта
    /// </summary>
    public record DetailedCourseThemeDtos(
        Guid Id,
        string Name,
        string Description,
        Guid TypeOfCourseId,
        ProgrammingLanguageDtos ProgrammingLanguage,
        List<CourseThemeRecommendationDtos> CourseThemeRecommendation,
        List<CourseModuleDtos>  CourseModules,
        int DisplayOrder,
        bool IsActive,
        bool IsFree);
}