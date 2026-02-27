namespace ContractualDtos.DTO.Course.CourseModule.Requests
{
    public record UpdateCourseModuleRequestDto(
        Guid Id,
        string Param,
        string Value);
}