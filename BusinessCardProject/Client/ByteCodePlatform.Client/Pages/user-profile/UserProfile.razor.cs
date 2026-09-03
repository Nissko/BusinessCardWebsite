using AuthorizationService.Proto;
using BusinessCardProject.Client.Entities.Services.Mains;
using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Pages.user_profile
{
    public partial class UserProfile : ComponentBase, IAsyncDisposable
    {
        [Inject] private ClientAuthenticationService AuthenticationService { get; set; } = null!;
        [Inject] private UserProfileService UserProfileService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        private UserInfoResponse? _userInfo;

        protected override void OnInitialized()
        {
            UserProfileService.OnUserProfileChanged += RefreshUserAvatarEvent;
        }

        protected override async Task OnInitializedAsync()
        {
            _userInfo = UserProfileService.UserInCache 
                        ?? await AuthenticationService.GetCurrentUser();
            
            if (_userInfo != null)
                UserProfileService.SetUserInCache(_userInfo);
        }

        private async Task RefreshUserAvatarEvent()
        {
            var isAuth = !string.IsNullOrEmpty(AuthenticationService.GetAccessToken());
            if (!isAuth) return;

            _userInfo = await AuthenticationService.GetCurrentUser();
            if (_userInfo != null) UserProfileService.SetUserInCache(_userInfo);

            StateHasChanged();
        }

        public ValueTask DisposeAsync()
        {
            try
            {
                UserProfileService.OnUserProfileChanged -= RefreshUserAvatarEvent;
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