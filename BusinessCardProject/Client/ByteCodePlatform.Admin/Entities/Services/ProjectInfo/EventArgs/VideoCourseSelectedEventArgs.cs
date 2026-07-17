using CourseService.Proto;

namespace ByteCodePlatform.Admin.Entities.Services.ProjectInfo.EventArgs
{
    public record VideoCourseSelectedEventArgs
    {
        public Guid CourseId { get; init; }
        public List<CourseContentInfoResponse> CourseContents { get; init; }
    }
}