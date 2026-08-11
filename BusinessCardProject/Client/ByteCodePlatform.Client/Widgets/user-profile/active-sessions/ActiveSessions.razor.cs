using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Widgets.user_profile.active_sessions
{
    public partial class ActiveSessions : ComponentBase
    {
        [Inject] private ClientAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        private List<SessionInfo> _sessions = new();
        private bool _isLoadingSessions;

        protected override async Task OnInitializedAsync()
        {
            await LoadActiveSessions();
        }

        private async Task LoadActiveSessions()
        {
            _isLoadingSessions = true;
            StateHasChanged();

            try
            {
                _sessions = await AuthenticationService.GetActiveSessions(10);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Ошибка загрузки сессий: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isLoadingSessions = false;
                StateHasChanged();
            }
        }

        private async Task RevokeSession(string tokenId)
        {
            try
            {
                await AuthenticationService.RevokeSession(tokenId);
                Snackbar.Add("Сессия закрыта", Severity.Success);
                await LoadActiveSessions();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Ошибка: {ex.Message}", Severity.Error);
            }
        }

        private async Task LogoutAll()
        {
            try
            {
                await AuthenticationService.LogoutAll();
                Snackbar.Add("Вы вышли со всех устройств", Severity.Success);
                NavManager.NavigateTo("/login", forceLoad: true);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Ошибка: {ex.Message}", Severity.Error);
            }
        }
    }
}