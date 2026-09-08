using NodaTime;

namespace DTOs.DTO.Course
{
    public record CheckGrpcCourseTimingDto(Instant DateTime, bool Health, string GrpcServiceName);
}