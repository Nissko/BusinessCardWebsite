using ByteCodePlatform.Admin.Features.admin.dynamic_edit_dialog;
using CourseService.Proto;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ByteCodePlatform.Admin.Features.admin.course_theme_editor
{
    public partial class CourseThemeEditor : ComponentBase
    {
        [Inject] private IDialogService Dialog { get; set; } = null!;

        [Parameter] public EventCallback OnDataChanged { get; set; }
        [Parameter] public List<CourseThemeInfoResponse>? CourseTheme { get; set; }
        [Parameter] public Guid? SelectedCtId { get; set; }
        [Parameter] public CourseThemePropertiesResponse? SelectedCtProperties { get; set; }

        private async Task OpenDialogAsync(
            Guid id,
            TypeOfEntityType entityType,
            string fieldName,
            DynamicInputType inputType,
            string currentValue,
            string label)
        {
            var parameters = new DialogParameters<DynamicEditDialog>
            {
                { x => x.RecordId, id },
                { x => x.EntityType, entityType },
                { x => x.FieldName, fieldName },
                { x => x.InputType, inputType },
                { x => x.InputValueString, currentValue },
                { x => x.LabelTextString, label },
                { x => x.ButtonTextString, "Изменить" }
            };

            var options = new DialogOptions { BackdropClick = false, FullWidth = true };
            var dialogReference = await Dialog.ShowAsync<DynamicEditDialog>("Редактирование", parameters, options);
            var dialogResult = await dialogReference.Result;

            if (!dialogResult!.Canceled && OnDataChanged.HasDelegate)
            {
                await OnDataChanged.InvokeAsync();
            }
        }
    }
}