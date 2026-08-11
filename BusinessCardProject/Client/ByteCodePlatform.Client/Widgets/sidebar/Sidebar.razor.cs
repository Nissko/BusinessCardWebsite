using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Widgets.sidebar
{
    public partial class Sidebar : ComponentBase, IDisposable
    {
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;
        [Inject] private IBrowserViewportService BrowserViewportService { get; set; } = null!;

        private Guid _viewportSubscriptionId;
        private readonly bool _overlayAutoClose = true;

        private async Task OnViewportChanged(BrowserViewportEventArgs args)
        {
            if (UserSettingService.Settings.IsDrawerOpen)
            {
                UserSettingService.Settings.IsDrawerOpen = false;
                UserSettingService.NotifyStateChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _viewportSubscriptionId = Guid.NewGuid();
            await BrowserViewportService.SubscribeAsync(
                _viewportSubscriptionId,
                OnViewportChanged,
                fireImmediately: true);
            
            UserSettingService.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            UserSettingService.OnChange -= StateHasChanged;
        }
    }
}