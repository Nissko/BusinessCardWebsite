using ByteCodePlatform.Admin.Features.admin.dynamic_edit_dialog;
using CourseService.Proto;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ByteCodePlatform.Admin.Features.admin.course_video_editor
{
    public partial class CourseVideoEditor : ComponentBase
    {
        [Inject] private IDialogService Dialog { get; set; } = null!;

        [Parameter] public EventCallback OnDataChanged { get; set; }
        [Parameter] public List<CourseContentInfoResponse>?  VideoCourses { get; set; }
        [Parameter] public CourseContentPropertiesResponse? CourseContentProperties { get; set; }
        [Parameter] public Guid? SelectedPreviewCourseContentId { get; set; }

        private CourseContentInfoResponse? _videoCourse;

        protected override Task OnParametersSetAsync()
        {
            _videoCourse = VideoCourses?
                .FirstOrDefault(vc => vc.Id == SelectedPreviewCourseContentId.ToString());
            
            return base.OnParametersSetAsync();
        }

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