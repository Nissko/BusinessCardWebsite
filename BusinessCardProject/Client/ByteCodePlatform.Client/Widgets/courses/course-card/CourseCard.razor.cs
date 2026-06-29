using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.course_card
{
    public partial class CourseCard : ComponentBase
    {
        [Parameter, EditorRequired]
        public CourseThemeInfoResponse Course { get; set; } = null!;

        [Parameter]
        public EventCallback<Guid> OnCourseSelected { get; set; }

        private async Task OnSelectClick()
        {
            if (Guid.TryParse(Course.Id, out var themeId))
            {
                await OnCourseSelected.InvokeAsync(themeId);
            }
        }

        private string GetFooterNote()
        {
            return Course.IsFree
                ? "* курс может включать в себя платный доп. контент"
                : "* доступ предоставляется навсегда";
        }
    }
}