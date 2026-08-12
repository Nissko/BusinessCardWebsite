using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.courses.course_card
{
    public partial class CourseCard : ComponentBase
    {
        [Parameter, EditorRequired] public CourseThemeInfoResponse Course { get; set; } = null!;
        [Parameter, EditorRequired] public List<CourseThemePropertiesResponse> CourseThemeProperties { get; set; } = null!;

        [Parameter] public EventCallback<Guid> OnCourseSelected { get; set; }

        private async Task OnSelectClick()
        {
            if (Guid.TryParse(Course.Id, out var themeId))
            {
                await OnCourseSelected.InvokeAsync(themeId);
            }
        }
    }
}