using AuthorizationService.Proto;
using BusinessCardProject.Client.Entities.Services.Mains;
using BusinessCardProject.Client.Widgets.user_profile.profile_header.avatar_upload_dialog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BusinessCardProject.Client.Widgets.user_profile.profile_header
{
    public partial class ProfileHeader : ComponentBase
    {
        [Parameter] public UserInfoResponse? UserInfoResponse { get; set; }
        [Parameter] public EventCallback<IBrowserFile> OnAvatarUpload { get; set; }

        [Inject] private GetLinksService GetLinksService { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        private async Task HandleAvatarClick()
        {
            if (UserInfoResponse is null) return;

            var currentUrl = string.IsNullOrEmpty(UserInfoResponse.UserAvatar)
                ? null
                : GetLinksService.GetUrlFromImageService(UserInfoResponse.UserAvatar);

            var parameters = new DialogParameters
            {
                { "CurrentAvatarUrl", currentUrl }
            };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<AvatarUploadDialog>(
                "Загрузка аватара", parameters, options);

            var result = await dialog.Result;

            if (result is { Canceled: false, Data: IBrowserFile file })
            {
                await OnAvatarUpload.InvokeAsync(file);
            }
        }
    }
}