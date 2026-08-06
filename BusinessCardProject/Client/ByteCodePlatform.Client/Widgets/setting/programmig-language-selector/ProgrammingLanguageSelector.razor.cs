using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Widgets.setting.programmig_language_selector
{
    public partial class ProgrammingLanguageSelector : ComponentBase
    {
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;

        private bool _isCSharpSelected;
        private bool _isPhpSelected;
        private bool _isLaravelSelected;

        private static Placement Placement => Placement.Bottom;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadSettingsAsync();
        }

        private async Task LoadSettingsAsync()
        {
            if (UserSettingService.Settings.ProgrammingLanguage == null)
            {
                await InitializeProgramLanguageAsync();
            }

            if (UserSettingService.Settings.ProgrammingLanguage != null)
            {
                UpdateSwitchesFromCache(UserSettingService.Settings.ProgrammingLanguage.ProgrammingLanguage);
            }
        }

        private async Task InitializeProgramLanguageAsync()
        {
            if (UserSettingService.Settings.ProgrammingLanguage is null)
            {
                UserSettingService.Settings.ProgrammingLanguage = new();
                UserSettingService.Settings.ProgrammingLanguage.SetProgrammingLanguage(0);
                await UserSettingService.Save();
            }
        }

        private async Task OnLanguageChanged(int languageIndex, bool isSelected)
        {
            if (!isSelected)
                return;

            _isCSharpSelected = false;
            _isPhpSelected = false;
            _isLaravelSelected = false;

            switch (languageIndex)
            {
                case 0:
                    _isCSharpSelected = true;
                    break;
                case 1:
                    _isPhpSelected = true;
                    break;
                case 2:
                    _isLaravelSelected = true;
                    break;
            }

            UserSettingService.Settings.ProgrammingLanguage?.SetProgrammingLanguage(languageIndex);
            await UserSettingService.Save();
        }

        private void UpdateSwitchesFromCache(int platformFromCache)
        {
            _isCSharpSelected = platformFromCache == 0;
            _isPhpSelected = platformFromCache == 1;
            _isLaravelSelected = platformFromCache == 2;
        }
    }
}