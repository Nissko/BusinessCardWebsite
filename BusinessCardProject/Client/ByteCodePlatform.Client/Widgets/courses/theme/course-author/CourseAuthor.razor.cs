using BusinessCardProject.Client.Entities.Services.Mains;
using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_author
{
    public partial class CourseAuthor : ComponentBase
    {
        [Parameter] public UserAuthorInfo Author { get; set; } = null!;
        [Inject] public GetLinksService GetLinksService { get; set; } = null!;
    }
}