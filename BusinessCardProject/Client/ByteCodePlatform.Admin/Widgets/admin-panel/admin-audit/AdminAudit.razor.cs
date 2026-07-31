/*using ByteCodePlatform.Admin.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ByteCodePlatform.Admin.Widgets.admin_panel.admin_audit
{
    public partial class AdminAudit : ComponentBase
    {
        [Inject] private ClientAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;
        private List<AuditLogItem> _logs = new();
        private bool _isLoading;
        private string _searchQuery = "";
        private int _totalCount;

        protected override async Task OnInitializedAsync()
        {
            await LoadLogs();
        }

        private async Task LoadLogs()
        {
            _isLoading = true;
            StateHasChanged();

            try
            {
                // TODO: вызвать Admin API для получения audit logs
                var response = await AuthenticationService.GetAuditLogs(_page, _pageSize, _searchQuery);
                _logs = response.Items;
                _totalCount = response.TotalCount;
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Ошибка загрузки логов: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }
    }

    public record AuditLogItem(Guid UserId, string Action, string? Details, DateTime CreatedAt);
}*/