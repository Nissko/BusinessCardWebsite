namespace ContractualDtos.DTO.Course.CourseTheme.Requests
{
    public record UpdateCourseThemeRequestDto(
        Guid Id,
        string Param,
        string Value);
}