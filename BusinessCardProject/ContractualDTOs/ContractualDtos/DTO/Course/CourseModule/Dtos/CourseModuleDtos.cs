using ContractualDtos.DTO.Course.VideoCourse.Dtos;

namespace ContractualDtos.DTO.Course.CourseModule.Dtos
{
    public record CourseModuleDtos(
        Guid Id,
        string Name,
        string Description,
        int DisplayOrder,
        bool IsShow,
        List<VideoCourseDtos> VideoCourses);
}