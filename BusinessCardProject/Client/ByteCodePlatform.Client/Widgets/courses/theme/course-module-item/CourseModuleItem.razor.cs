using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.theme.course_module_item
{
    public partial class CourseModuleItem : ComponentBase
    {
        [Parameter, EditorRequired]
        public CourseModuleInfoResponse Module { get; set; } = null!;
        [Parameter]
        public CourseService.Proto.CourseService.CourseServiceClient CourseGrpc { get; set; } = null!;
    }
}