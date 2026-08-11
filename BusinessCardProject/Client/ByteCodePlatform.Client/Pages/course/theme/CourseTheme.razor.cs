using BusinessCardProject.Client.Entities.Enums;
using BusinessCardProject.Client.Entities.Services.ProjectInfo;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Pages.course.theme;

public partial class CourseTheme : ComponentBase
{
    [Inject] private UserSettingService UserSettingService { get; set; } = null!;
    private string _currentProgramLanguage = "";
    
    protected override async Task OnInitializedAsync()
    {
        var language = UserSettingService.Settings.ProgrammingLanguage?.SelectedProgrammingLanguage ?? 0;
        _currentProgramLanguage = ProgrammingLanguageCategoryEnum.GetNameFromSequenceNumber(language);
    }
}