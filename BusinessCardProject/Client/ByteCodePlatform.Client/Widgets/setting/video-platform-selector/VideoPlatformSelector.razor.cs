using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Widgets.setting.video_platform_selector
{
    public partial class VideoPlatformSelector : ComponentBase
    {
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;

        private bool _isYouTubeSelected;
        private bool _isRuTubeSelected;
        private bool _isVkVideoSelected;

        private static Placement Placement => Placement.Bottom;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadSettingsAsync();
        }

        private async Task LoadSettingsAsync()
        {
            if (UserSettingService.Settings.VideoPlatform == null)
            {
                await InitializeVideoPlatformAsync();
            }

            if (UserSettingService.Settings.VideoPlatform != null)
            {
                UpdateSwitchesFromCache(UserSettingService.Settings.VideoPlatform.Platform);
            }
        }

        private async Task InitializeVideoPlatformAsync()
        {
            if (UserSettingService.Settings.VideoPlatform is null)
            {
                UserSettingService.Settings.VideoPlatform = new();
                UserSettingService.Settings.VideoPlatform.SetPlatform(0);
                await UserSettingService.Save();
            }
        }

        private async Task OnPlatformChanged(int platformIndex, bool isSelected)
        {
            if (!isSelected)
                return;

            _isYouTubeSelected = false;
            _isRuTubeSelected = false;
            _isVkVideoSelected = false;

            switch (platformIndex)
            {
                case 0:
                    _isYouTubeSelected = true;
                    break;
                case 1:
                    _isRuTubeSelected = true;
                    break;
                case 2:
                    _isVkVideoSelected = true;
                    break;
            }

            UserSettingService.Settings.VideoPlatform?.SetPlatform(platformIndex);
            await UserSettingService.Save();
        
            // TODO: Отправить запрос на изменение настроек в БД
        }

        private void UpdateSwitchesFromCache(int platformFromCache)
        {
            _isYouTubeSelected = platformFromCache == 0;
            _isRuTubeSelected = platformFromCache == 1;
            _isVkVideoSelected = platformFromCache == 2;
        }
    }
}