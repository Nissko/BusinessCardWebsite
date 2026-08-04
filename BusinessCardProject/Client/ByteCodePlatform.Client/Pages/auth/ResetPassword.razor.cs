using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using BusinessCardProject.Client.Features.auth.reset_password_form;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Severity = MudBlazor.Severity;

namespace BusinessCardProject.Client.Pages.auth
{
    public partial class ResetPassword : ComponentBase
    {
        [Inject] private ClientAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        [Parameter] public string UserId { get; set; } = "";
        [Parameter] public string ResetToken { get; set; } = "";

        private MudForm _form = null!;
        private readonly ResetPasswordModel _model = new();
        private readonly ResetPasswordModelValidator _validator = new();

        private bool _success;
        private string _errorMessage = "";
        private bool _isLoading;

        private InputType _passwordInputType = InputType.Password;
        private string _passwordIcon = Icons.Material.Rounded.VisibilityOff;

        private InputType _confirmPasswordInputType = InputType.Password;
        private string _confirmPasswordIcon = Icons.Material.Rounded.VisibilityOff;

        private void TogglePasswordVisibility()
        {
            _passwordInputType = _passwordInputType == InputType.Password ? InputType.Text : InputType.Password;
            _passwordIcon = _passwordInputType == InputType.Password
                ? Icons.Material.Rounded.VisibilityOff
                : Icons.Material.Rounded.Visibility;
        }

        private void ToggleConfirmPasswordVisibility()
        {
            _confirmPasswordInputType =
                _confirmPasswordInputType == InputType.Password ? InputType.Text : InputType.Password;
            _confirmPasswordIcon = _confirmPasswordInputType == InputType.Password
                ? Icons.Material.Rounded.VisibilityOff
                : Icons.Material.Rounded.Visibility;
        }

        private async Task OnNewPasswordChanged(string value)
        {
            _model.NewPassword = value;

            await Task.Delay(10);

            if (_form != null)
            {
                await _form.Validate();
            }
        }

        private async Task OnConfirmPasswordChanged(string value)
        {
            _model.ConfirmPassword = value;

            await Task.Delay(10);

            if (_form != null)
            {
                await _form.Validate();
            }
        }

        private async Task HandleReset()
        {
            _errorMessage = "";
            await _form.Validate();

            if (!_form.IsValid)
            {
                Snackbar.Add("Проверьте правильность заполнения полей", Severity.Warning);
                return;
            }

            _isLoading = true;
            StateHasChanged();

            try
            {
                var result =
                    await AuthenticationService.ResetPassword(Guid.Parse(UserId), ResetToken, _model.NewPassword);
                if (result)
                {
                    _success = true;
                    Snackbar.Add("Пароль успешно изменён", Severity.Success);
                }
                else
                {
                    _errorMessage = "Не удалось сбросить пароль. Ссылка могла истечь или быть недействительной.";
                }
            }
            catch (FormatException)
            {
                _errorMessage = "Неверный формат ссылки для восстановления.";
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