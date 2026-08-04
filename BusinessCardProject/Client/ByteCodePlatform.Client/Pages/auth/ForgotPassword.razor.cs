using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Pages.auth
{
    public partial class ForgotPassword : ComponentBase
    {
        [Inject] private ClientAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        private string _email = "";
        private bool _success;
        private string _errorMessage = "";
        private bool _isLoading;

        private async Task HandleSendReset()
        {
            _errorMessage = "";
            _isLoading = true;
            StateHasChanged();

            try
            {
                var result = await AuthenticationService.SendPasswordReset(_email);
                if (result)
                    _success = true;
                else
                    _errorMessage = "Не удалось отправить письмо. Проверьте правильность email.";
            }
            catch (Exception ex)
            {
                _errorMessage = $"Ошибка: {ex.Message}";
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }
    }
}