using ByteCodePlatform.Admin.Entities.Services.ProjectInfo;
using ByteCodePlatform.Admin.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Severity = MudBlazor.Severity;

namespace ByteCodePlatform.Admin.Features.auth.login_form
{
    public partial class LoginForm : ComponentBase, IDisposable
    {
        [Inject] private ClientAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;

        [Parameter] public EventCallback OnRegisterClick { get; set; }

        private MudForm _form = null!;
        private readonly LoginModel _model = new();
        private readonly LoginModelValidator _validator = new();

        private string _errorMessage = "";
        private bool _showError;
        private bool _isLoading;
        private bool _isSettingsLoaded;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            UserSettingService.OnChange += StateHasChanged;
        
            if (UserSettingService.Settings.UpdateTime == null)
            {
                await UserSettingService.LoadAsync();
                UserSettingService.Settings.UpdateTime = DateTime.Now;
                await UserSettingService.SaveAsync();
            }
        
            _isSettingsLoaded = true;
        }

        private async Task HandleLogin()
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
                var successAuth = await AuthenticationService.Login(_model.Email, _model.Password);

                if (successAuth)
                {
                    Snackbar.Add("Успешный вход!", Severity.Success);
                    NavManager.NavigateTo("/", forceLoad: true);
                }
                else
                {
                    _errorMessage = "Неверный логин или пароль";
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
                await HandleLogin();
            }
        }

        public void Dispose()
        {
            UserSettingService.OnChange -= StateHasChanged;
        }
    }
}