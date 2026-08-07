using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Severity = MudBlazor.Severity;

namespace BusinessCardProject.Client.Features.auth.register_form
{
    public partial class RegisterForm : ComponentBase, IDisposable
    {
        [Inject] private ClientAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        private MudForm _form = null!;
        private readonly RegisterModel _model = new();
        private readonly RegisterModelValidator _validator = new();

        private string _errorMessage = "";
        private bool _showError;
        private bool _isLoading;

        private InputType _passwordInputType = InputType.Password;
        private string _passwordIcon = Icons.Material.Rounded.VisibilityOff;

        private void OnBackClick()
        {
            NavManager.NavigateTo("/login");
        }

        private async Task HandleRegister()
        {
            _showError = false;
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
                var success = await AuthenticationService.Register(
                    _model.Surname,
                    _model.Name,
                    _model.NickName,
                    _model.Email,
                    _model.Password);

                if (success)
                {
                    Snackbar.Add("Подтвердите аккаунт через свою почту", Severity.Success);
                    NavManager.NavigateTo("/login");
                }
                else
                {
                    _errorMessage = "Ошибка регистрации. Возможно, пользователь с такой почтой или никнеймом уже существует";
                    _showError = true;
                    Snackbar.Add(_errorMessage, Severity.Error);
                }
            }
            catch (Exception ex) when (ex.Message.Contains("временно заблокирован"))
            {
                _errorMessage = ex.Message;
                _showError = true;
                Snackbar.Add(_errorMessage, Severity.Error);
            }
            catch (Exception ex)
            {
                _errorMessage = $"Ошибка подключения: {ex.Message}";
                _showError = true;
                Snackbar.Add(_errorMessage, Severity.Error);
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private async Task HandleEnter(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !_isLoading)
            {
                await HandleRegister();
            }
        }

        private void TogglePasswordVisibility()
        {
            if (_passwordInputType == InputType.Password)
            {
                _passwordInputType = InputType.Text;
                _passwordIcon = Icons.Material.Rounded.Visibility;
            }
            else
            {
                _passwordInputType = InputType.Password;
                _passwordIcon = Icons.Material.Rounded.VisibilityOff;
            }
        }

        public void Dispose() { }
    }
}