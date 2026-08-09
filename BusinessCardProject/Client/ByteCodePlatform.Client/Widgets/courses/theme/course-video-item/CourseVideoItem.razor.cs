using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_video_item
{
    public partial class CourseVideoItem : ComponentBase
    {
        [Parameter, EditorRequired]
        public CourseContentInfoResponse VideoCourse { get; set; } = null!;
        private string PreviewUrl => $"https://localhost:5036/FilesServiceGrpcService/{VideoCourse.ImgUrl}";
        //private string PreviewUrl => $"https://it-bytecode.splinterkeenetic.netcraze.club/FilesServiceGrpcService/{VideoCourse.ImgUrl}";

        [Parameter, EditorRequired]
        public int Index { get; set; }

        private static void OnWatchClick()
        {
            // TODO: Реализовать переход к просмотру видео
        }

        private static void OnFavoriteClick()
        {
            // TODO: Реализовать добавление в избранное
        }
    }
}