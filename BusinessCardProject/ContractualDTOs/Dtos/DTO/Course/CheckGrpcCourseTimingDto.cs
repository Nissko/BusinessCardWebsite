using NodaTime;

namespace Dtos.DTO.Course
{
    public record CheckGrpcCourseTimingDto(Instant DateTime, bool Health, string GrpcServiceName);
}