namespace DTOs.DTO.Course.ProgramLanguage
{
    public record ProgrammingLanguageDto(
        Guid Id,
        string Name,
        List<LightCourseThemeDto> CourseThemes);
}