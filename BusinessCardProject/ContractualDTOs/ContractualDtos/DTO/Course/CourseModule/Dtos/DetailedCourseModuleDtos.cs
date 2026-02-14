using ContractualDtos.DTO.Course.CourseTheme.Dtos;

namespace ContractualDtos.DTO.Course.CourseModule.Dtos
{
    /// <summary>
    /// DTO для возврата объекта
    /// </summary>
    public record DetailedCourseModuleDtos(
        Guid Id,
        string Name,
        string Description,
        CourseThemeDtos CourseTheme);
}