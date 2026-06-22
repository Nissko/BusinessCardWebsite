using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Widgets.sidebar
{
    public partial class Sidebar : ComponentBase, IDisposable
    {
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;

        protected override void OnInitialized()
        {
            UserSettingService.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            UserSettingService.OnChange -= StateHasChanged;
        }
    }
}