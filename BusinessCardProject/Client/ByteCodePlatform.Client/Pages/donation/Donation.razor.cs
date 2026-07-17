using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Pages.donation;

public partial class Donation : ComponentBase, IDisposable
{
    [Inject] private UserSettingService UserSettingService { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        UserSettingService.OnChange += StateHasChanged;
        if (UserSettingService.Settings.UpdateTime == null)
        {
            await UserSettingService.LoadAsync();
            UserSettingService.Settings.UpdateTime = DateTime.Now;
            await UserSettingService.SaveAsync();
        }
    }
    
    public void Dispose()
    {
        UserSettingService.OnChange -= StateHasChanged;
    }
}