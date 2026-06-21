using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Widgets.header
{
    public partial class Header : IDisposable
    {
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;

        private Color HeaderColor => UserSettingService.Settings.IsDark ? Color.Info : Color.Dark;

        protected override async Task OnInitializedAsync()
        {
            UserSettingService.OnChange += StateHasChanged;
            if (UserSettingService.Settings.UpdateTime == null)
            {
                await UserSettingService.LoadAsync();
                UserSettingService.Settings.UpdateTime = DateTime.Now;
                await UserSettingService.SaveAsync();
            }

            StateHasChanged();
        }

        private void ToggleDrawer()
        {
            UserSettingService.Settings.IsDrawerOpen = !UserSettingService.Settings.IsDrawerOpen;
            UserSettingService.NotifyStateChanged(); 
        }
    
        public void Dispose()
        {
            UserSettingService.OnChange -= StateHasChanged;
        }
    }
}