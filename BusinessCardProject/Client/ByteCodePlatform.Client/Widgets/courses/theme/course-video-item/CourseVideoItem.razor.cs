using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_video_item
{
    public partial class CourseVideoItem : ComponentBase
    {
        [Parameter, EditorRequired]
        public CourseContentInfoResponse VideoCourse { get; set; } = null!;
        private string PreviewUrl => $"https://localhost:7146/{VideoCourse.ImgUrl}";
        //private string PreviewUrl => $"https://it-bytecode.splinterkeenetic.netcraze.club/FileGrpcService/{VideoCourse.ImgUrl}";

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