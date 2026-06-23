using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_video_item
{
    public partial class CourseVideoItem : ComponentBase
    {
        [Parameter, EditorRequired]
        public CourseContentInfoResponse VideoCourse { get; set; } = null!;

        [Parameter, EditorRequired]
        public int Index { get; set; }

        private void OnWatchClick()
        {
            // TODO: Реализовать переход к просмотру видео
        }

        private void OnFavoriteClick()
        {
            // TODO: Реализовать добавление в избранное
        }
    }
}