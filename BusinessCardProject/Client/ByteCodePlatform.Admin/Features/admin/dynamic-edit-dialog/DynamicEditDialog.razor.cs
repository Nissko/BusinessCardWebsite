using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ByteCodePlatform.Admin.Features.admin.dynamic_edit_dialog
{
    public partial class DynamicEditDialog : ComponentBase
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
        //[Inject] private ICoreBaseService CoreBaseService { get; set; } = null!;

        [Parameter, EditorRequired] public string LabelTextString { get; set; } = string.Empty;
        [Parameter, EditorRequired] public Guid RecordId { get; set; }
        [Parameter, EditorRequired] public TypeOfEntityType NameOfApi { get; set; }
        [Parameter, EditorRequired] public string NameOfApiKey { get; set; } = string.Empty;
        [Parameter, EditorRequired] public string ButtonTextString { get; set; } = string.Empty;
        [Parameter, EditorRequired] public DynamicInputType InputType { get; set; }
        [Parameter, EditorRequired] public string InputValueString { get; set; } = string.Empty;

        private int? _currentNumberValue;
        private string? _currentBoolValue;
        private bool _isLoading;

        protected override void OnInitialized()
        {
            switch (InputType)
            {
                case DynamicInputType.Number when int.TryParse(InputValueString, out var num):
                    _currentNumberValue = num;
                    break;
                case DynamicInputType.Boolean when bool.TryParse(InputValueString, out var boolVal):
                    _currentBoolValue = boolVal ? "true" : "false";
                    break;
            }
        }

        private void Cancel() => MudDialog.Cancel();

        private async Task UpdateData()
        {
            if (_isLoading) return;

            var valueToSend = InputType switch
            {
                DynamicInputType.Number => _currentNumberValue?.ToString(),
                DynamicInputType.Boolean => _currentBoolValue,
                _ => InputValueString
            };

            if (string.IsNullOrEmpty(valueToSend) && InputType != DynamicInputType.Boolean)
                return;

            _isLoading = true;
            StateHasChanged();

            try
            {
                /*TODO: Переделать*/
                /*var payload = new DynamicClassDto(RecordId, NameOfApi.ToString(), NameOfApiKey, valueToSend!);
                var success = await CoreBaseService.DynamicUpdateAsync(payload);*/

                /*if (success)
                    MudDialog.Close(DialogResult.Ok(true));*/
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }
    }
}