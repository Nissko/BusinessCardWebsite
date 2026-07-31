using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Pages.auth
{
    public partial class VerificationAccount : ComponentBase
    {
        [Parameter] public Guid UserId { get; set; }
        [Parameter] public string VerificationToken { get; set; } = string.Empty;

        [Inject] private ClientAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                var isSuccess = await AuthenticationService.VerificationAccount(UserId, VerificationToken);

                if (isSuccess)
                {
                    Snackbar.Add("Аккаунт успешно подтвержден!", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Неверный код верификации, либо аккаунт уже подтвержден.", Severity.Error);
                }
            }
            catch
            {
                Snackbar.Add("Критическая ошибка. Свяжитесь с администратором системы.", Severity.Error);
            }
            finally
            {
                NavManager.NavigateTo("/login");
            }
        }
    }
}