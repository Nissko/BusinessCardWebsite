using ByteCodePlatform.Admin.Entities.Services.ProjectInfo.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ByteCodePlatform.Admin.Features.admin.dynamic_edit_dialog
{
    public partial class DynamicEditDialog : ComponentBase
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
        [Inject] private IEntityUpdateService UpdateService { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        [Parameter, EditorRequired] public string LabelTextString { get; set; } = string.Empty;
        [Parameter, EditorRequired] public Guid RecordId { get; set; }
        [Parameter, EditorRequired] public TypeOfEntityType EntityType { get; set; }
        [Parameter, EditorRequired] public string FieldName { get; set; } = string.Empty;
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

            // Определяем значение для отправки
            object? valueToSend = InputType switch
            {
                DynamicInputType.Number => _currentNumberValue,
                DynamicInputType.Boolean => bool.TryParse(_currentBoolValue, out var b) ? b : null,
                _ => InputValueString
            };

            // Валидация
            if (valueToSend == null || (valueToSend is string s && string.IsNullOrEmpty(s)))
            {
                if (InputType != DynamicInputType.Boolean)
                {
                    Snackbar.Add("Значение не должно быть пустым", Severity.Warning);
                    return;
                }
            }

            _isLoading = true;
            StateHasChanged();

            try
            {
                var success = await UpdateService.UpdateFieldAsync(EntityType, RecordId, FieldName, valueToSend);

                if (success)
                {
                    Snackbar.Add("Изменения сохранены", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(true));
                }
                else
                {
                    Snackbar.Add("Ошибка при сохранении", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Ошибка: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }
    }
}