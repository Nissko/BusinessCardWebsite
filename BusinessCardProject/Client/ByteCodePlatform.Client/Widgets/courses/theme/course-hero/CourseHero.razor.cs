using BusinessCardProject.Client.Entities.Services.Mains;
using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_hero
{
    public partial class CourseHero : ComponentBase
    {
        [Parameter, EditorRequired]
        public CourseThemeInfoResponse Course { get; set; } = null!;
        [Inject] private GetLinksService GetLinksService { get; set; } = null!;
    }
}