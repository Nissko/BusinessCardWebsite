using ByteCodePlatform.Admin.Features.admin.dynamic_edit_dialog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ByteCodePlatform.Admin.Features.admin.course_theme_editor
{
    public partial class CourseThemeEditor : ComponentBase
    {
        [Inject] private IDialogService Dialog { get; set; } = null!;

        [Parameter] public EventCallback OnDataChanged { get; set; }
        //[Parameter] public IEnumerable<DetailedCourseThemeDtos>? CourseTheme { get; set; }
        [Parameter] public string? SelectedCt { get; set; }

        private readonly DialogOptions _backdropClick = new() { BackdropClick = false, FullWidth = true };

        private async Task OpenDialogAsync(Guid id, DialogParameters<DynamicEditDialog> parameters, DialogOptions options)
        {
            const string typeName = "Редактирование";
            parameters.Add(x => x.ButtonTextString, "Изменить");
            parameters.Add(x => x.RecordId, id);
            parameters.Add(x => x.NameOfApi, TypeOfEntityType.CourseThemes);

            var dialogReference = await Dialog.ShowAsync<DynamicEditDialog>(typeName, parameters, options);
            var dialogResult = await dialogReference.Result;

            if (!dialogResult!.Canceled && OnDataChanged.HasDelegate)
            {
                await OnDataChanged.InvokeAsync();
            }
        }
    }
}