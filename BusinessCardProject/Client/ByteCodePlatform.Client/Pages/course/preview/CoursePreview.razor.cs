using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Pages.course.preview
{
    public partial class CoursePreview : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }
        private CourseThemeInfoResponse? CourseThemeInfo { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var request = new GetCourseThemeRequest { Id = Id.ToString() };
            CourseThemeInfo = await CourseGrpc.GetCourseThemeAsync(request);
        }
    }
}