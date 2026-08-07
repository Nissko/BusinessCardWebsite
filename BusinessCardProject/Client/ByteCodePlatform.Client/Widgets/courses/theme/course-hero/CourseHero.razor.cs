using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_hero
{
    public partial class CourseHero : ComponentBase
    {
        [Parameter, EditorRequired]
        public CourseThemeInfoResponse Course { get; set; } = null!;

        private string PreviewUrl => $"https://localhost:7146/{Course.AvatarUrl}";
        //private string PreviewUrl => $"https://it-bytecode.splinterkeenetic.netcraze.club/FileGrpcService/{Course.AvatarUrl}";
    }
}