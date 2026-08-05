using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Widgets.header
{
    public partial class Header : ComponentBase, IDisposable
    {
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;
        [Inject] private IBrowserViewportService BrowserViewportService { get; set; } = null!;

        private Guid _viewportSubscriptionId;
        private bool _showHeaderButtons;

        protected override async Task OnInitializedAsync()
        {
            UserSettingService.OnChange += StateHasChanged;
            
            _viewportSubscriptionId = Guid.NewGuid();
            await BrowserViewportService.SubscribeAsync(
                _viewportSubscriptionId,
                OnViewportChanged,
                fireImmediately: true);

            StateHasChanged();
        }

        private async Task OnViewportChanged(BrowserViewportEventArgs args)
        {
            _showHeaderButtons = CheckBreakPoint(args.Breakpoint);
            await InvokeAsync(StateHasChanged);
        }
        
        private static bool CheckBreakPoint(Breakpoint breakpoint)
        {
            return breakpoint switch
            {
                Breakpoint.Xxl => true,
                Breakpoint.Xl or Breakpoint.Lg or Breakpoint.Md => true,
                _ => false
            };
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