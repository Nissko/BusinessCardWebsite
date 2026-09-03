using BusinessCardProject.Client.Entities.Services.Mains;
using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_video_item
{
    public partial class CourseVideoItem : ComponentBase
    {
        [Parameter, EditorRequired]
        public CourseContentInfoResponse VideoCourse { get; set; } = null!;
        
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private GetLinksService GetLinksService { get; set; } = null!;
        
        /// <summary>
        /// Ссылка для просмотра видео
        /// </summary>
        private string Link => OnWatchLink();

        [Parameter, EditorRequired]
        public int Index { get; set; }

        private string OnWatchLink()
        {
            var watchingPlatform = UserSettingService.Settings.VideoPlatform?.Platform ?? 0;
            var link = watchingPlatform switch
            {
                0 => VideoCourse.LinkYoutube,
                1 => VideoCourse.LinkRutube,
                2 => VideoCourse.LinkVk,
                _ => VideoCourse.LinkYoutube
            };

            return link;
        }

        private static void OnFavoriteClick()
        {
            // TODO: Реализовать добавление в избранное
        }
    }
}