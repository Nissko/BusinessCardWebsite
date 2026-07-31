using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Pages.auth
{
    public partial class ResetPassword : ComponentBase
    {
        [Inject] private ClientAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        [Parameter] public string UserId { get; set; } = "";
        [Parameter] public string ResetToken { get; set; } = "";

        private string _newPassword = "";
        private string _confirmPassword = "";
        private bool _success;
        private string _errorMessage = "";
        private string _passwordErrors = "";
        private bool _isLoading;

        private async Task HandleReset()
        {
            _errorMessage = "";
            _passwordErrors = "";

            if (_newPassword != _confirmPassword)
            {
                _errorMessage = "Пароли не совпадают";
                return;
            }

            _isLoading = true;
            StateHasChanged();

            try
            {
                var result = await AuthenticationService.ResetPassword(Guid.Parse(UserId), ResetToken, _newPassword);
                if (result)
                {
                    _success = true;
                }
                else
                {
                    _errorMessage = "Не удалось сбросить пароль. Токен мог истечь.";
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }
    }
}