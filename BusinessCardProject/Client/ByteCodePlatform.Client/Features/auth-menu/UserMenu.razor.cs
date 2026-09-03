using AuthorizationService.Proto;
using BusinessCardProject.Client.Entities.Services.Mains;
using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Features.auth_menu
{
    public partial class UserMenu : ComponentBase, IAsyncDisposable
    {
        [Inject] private ClientAuthenticationService ClientAuthentication { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient GrpcClient { get; set; } = null!;
        [Inject] private GetLinksService GetLinksService { get; set; } = null!;
        [Inject] private UserProfileService UserProfileService { get; set; } = null!;

        private bool IsAuthenticated { get; set; }
        private string UserName { get; set; } = string.Empty;
        private string UserEmail { get; set; } = string.Empty;
        private string UserAvatar { get; set; } = string.Empty;

        private MudMenu? _menu;
        private int _cntLoadUserInfo;

        protected override async Task OnInitializedAsync()
        {
            IsAuthenticated = !string.IsNullOrEmpty(ClientAuthentication.GetAccessToken());
            UserProfileService.OnUserProfileChanged += RefreshUserAvatarEvent;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && IsAuthenticated)
            {
                if (UserProfileService.UserInCache != null)
                {
                    UpdateUserInfoFromResponse(UserProfileService.UserInCache);
                }
                else
                {
                    await LoadUserInfo();
                }

                StateHasChanged();
            }
        }

        private async Task RefreshUserAvatarEvent()
        {
            if (!IsAuthenticated) return;

            var response = await GrpcClient.GetCurrentUserAsync(new GetCurrentUserRequest { });
            UserProfileService.SetUserInCache(response);

            UpdateUserInfoFromResponse(response);
            StateHasChanged();
        }

        private async Task LoadUserInfo()
        {
            try
            {
                var response = await GrpcClient.GetCurrentUserAsync(new GetCurrentUserRequest { });

                UserName = !string.IsNullOrEmpty(response.Name)
                    ? response.Name
                    : (response.Nickname ?? response.Email?.Split('@').FirstOrDefault() ?? "User");

                UserEmail = response.Email ?? string.Empty;

                if (!string.IsNullOrEmpty(response.UserId))
                {
                    UserAvatar = GetLinksService.GetUrlFromImageService(response.UserAvatar);
                }
            }
            catch
            {
                UserName = "Пользователь";
                UserEmail = string.Empty;

                // Пробуем подгрузить спустя время
                if (_cntLoadUserInfo <= 5)
                {
                    _cntLoadUserInfo++;
                    await Task.Delay(500);
                    await LoadUserInfo();
                }
            }
        }

        private void UpdateUserInfoFromResponse(UserInfoResponse response)
        {
            UserName = response.Name;
            UserEmail = response.Email;
            UserAvatar = GetLinksService.GetUrlFromImageService(response.UserAvatar);
        }

        private async Task GoToProfile()
        {
            if (_menu is not null)
            {
                await _menu.CloseMenuAsync();
            }

            NavManager.NavigateTo("/user-profile");
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

        public ValueTask DisposeAsync()
        {
            try
            {
                UserProfileService.OnUserProfileChanged -= RefreshUserAvatarEvent;
                UserName = string.Empty;
                UserEmail = string.Empty;
                UserAvatar = string.Empty;
                IsAuthenticated = false;
                _cntLoadUserInfo = 0;
                GC.SuppressFinalize(this);
                return ValueTask.CompletedTask;
            }
            catch (Exception exception)
            {
                return ValueTask.FromException(exception);
            }
        }
    }
}