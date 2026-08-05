using AuthorizationService.Proto;
using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Features.auth_menu
{
    public partial class UserMenu : ComponentBase, IAsyncDisposable
    {
        [Inject] private ClientAuthenticationService ClientAuthentication { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private UserSettingService UserSettings { get; set; } = null!;
        [Inject] private AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient GrpcClient { get; set; } = null!;

        private bool IsAuthenticated { get; set; }
        private string UserName { get; set; } = string.Empty;
        private string UserEmail { get; set; } = string.Empty;
        private string UserAvatar { get; set; } = string.Empty;

        protected override void OnInitialized()
        {
            IsAuthenticated = !string.IsNullOrEmpty(ClientAuthentication.GetToken());
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && IsAuthenticated)
            {
                await LoadUserInfo();
                StateHasChanged();
            }
        }

        private async Task LoadUserInfo()
        {
            try
            {
                var response = await GrpcClient.GetCurrentUserAsync(new GetCurrentUserRequest
                {
                    AccessToken = ClientAuthentication.GetAccessToken()
                });

                UserName = !string.IsNullOrEmpty(response.Name)
                    ? response.Name
                    : (response.Nickname ?? response.Email?.Split('@').FirstOrDefault() ?? "User");

                UserEmail = response.Email ?? string.Empty;

                if (!string.IsNullOrEmpty(response.UserId))
                {
                    var hash = Convert.ToHexString(
                        System.Security.Cryptography.SHA256.HashData(
                            System.Text.Encoding.UTF8.GetBytes(response.UserId)));
                    UserAvatar = $"https://www.gravatar.com/avatar/{hash}?d=identicon&s=40";
                }
            }
            catch
            {
                UserName = "Пользователь";
                UserEmail = string.Empty;
            }
        }

        private void OnLoginClick()
        {
            NavManager.NavigateTo("/login");
        }

        private async void OnLogoutClick()
        {
            await ClientAuthentication.Logout();
            IsAuthenticated = false;
            UserName = string.Empty;
            UserEmail = string.Empty;
            UserAvatar = string.Empty;
            StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            // ignored 
        }
    }
}