using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using BusinessCardProject.Client.Shared.config.theme;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace BusinessCardProject.Client.Features.toggle_theme
{
    public partial class ThemeToggle : ComponentBase, IDisposable
    {
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;
        [Inject] private IJSRuntime Js { get; set; } = null!;

        protected override void OnInitialized()
        {
            UpdateThemeProperties();
            UserSettingService.OnChange += StateHasChanged;
        }

        private async Task ToggleTheme()
        {
            UserSettingService.Settings.IsDark = !UserSettingService.Settings.IsDark;
            UpdateThemeProperties();

            await Js.InvokeVoidAsync("themeHelper.setTheme", UserSettingService.Settings.IsDark);
            await UserSettingService.Save();
        }

        private static void UpdateThemeProperties()
        {
            new MudTheme
            {
                PaletteLight = Palettes.Light,
                PaletteDark = Palettes.Dark,
                LayoutProperties = new LayoutProperties()
            };
        }

        private string ButtonIcon => UserSettingService.Settings.IsDark
            ? Icons.Material.Filled.WbSunny
            : Icons.Material.Outlined.DarkMode;

        public void Dispose()
        {
            UserSettingService.OnChange -= StateHasChanged;
        }
    }
}