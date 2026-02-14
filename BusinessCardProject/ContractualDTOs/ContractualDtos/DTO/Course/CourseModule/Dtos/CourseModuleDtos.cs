using ContractualDtos.DTO.Course.VideoCourse.Dtos;

namespace ContractualDtos.DTO.Course.CourseModule.Dtos
{
    public record CourseModuleDtos(
        Guid Id,
        string Name,
        string Description,
        List<VideoCourseDtos> VideoCourses);
}