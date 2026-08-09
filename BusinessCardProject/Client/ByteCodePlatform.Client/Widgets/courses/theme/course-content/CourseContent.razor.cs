using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_content
{
    public partial class CourseContent : ComponentBase
    {
        [Parameter, EditorRequired] public CourseThemeInfoResponse Course { get; set; } = null!;
        [Parameter] public CourseService.Proto.CourseService.CourseServiceClient CourseGrpc { get; set; } = null!;
        private CourseModulesInfoResponse? CourseModules { get; set; }
        private bool _showWarning;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Course.CountLessons != 0)
            {
                var request = new GetCourseModulesByCourseIdRequest { CourseId = Course.Id };
                CourseModules = await CourseGrpc.GetCourseModulesByCourseIdAsync(request);
            }
            else
            {
                _showWarning = true;
            }
        }
    }
}