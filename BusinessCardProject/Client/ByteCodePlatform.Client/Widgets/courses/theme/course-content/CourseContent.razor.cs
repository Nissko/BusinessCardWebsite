using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_content
{
    public partial class CourseContent : ComponentBase
    {
        [Parameter, EditorRequired]
        public CourseThemeInfoResponse Course { get; set; } = null!;
        [Parameter]
        public CourseService.Proto.CourseService.CourseServiceClient CourseGrpc { get; set; } = null!;
        private CourseModulesInfoResponse? CourseModules { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var request = new GetCourseModulesFromCourseRequest { CourseId = Course.Id };
            CourseModules = await CourseGrpc.GetCourseModulesFromCourseAsync(request);
        }
    }
}