using BusinessCardProject.Client.Entities.Services.Mains;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Pages.donation
{
    public partial class Donation : ComponentBase, IDisposable
    {
        [Inject] private UserSettingService UserSettingService { get; set; } = null!;
    
        protected override async Task OnInitializedAsync()
        {
            UserSettingService.OnChange += StateHasChanged;
        }
    
        public void Dispose()
        {
            UserSettingService.OnChange -= StateHasChanged;
        }
    }
}