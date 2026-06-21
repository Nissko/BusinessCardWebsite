using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Features.auth_menu
{
    public partial class UserMenu : ComponentBase
    {
        [Inject] private ClientAuthenticationService ClientAuthentication { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;

        private bool IsAuthenticated { get; set; }

        protected override void OnInitialized()
        {
            IsAuthenticated = !string.IsNullOrEmpty(ClientAuthentication.GetToken());
        }

        private void OnLoginClick()
        {
            NavManager.NavigateTo("/login");
        }

        private void OnLogoutClick()
        {
            _ = ClientAuthentication.Logout(ClientAuthentication.GetToken());
            IsAuthenticated = false;
            OnLoginClick();
        }
    }
}