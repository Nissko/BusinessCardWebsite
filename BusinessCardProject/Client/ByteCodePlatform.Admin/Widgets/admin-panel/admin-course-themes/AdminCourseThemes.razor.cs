using Microsoft.AspNetCore.Components;

namespace ByteCodePlatform.Admin.Widgets.admin_panel.admin_course_themes
{
    public partial class AdminCourseThemes : ComponentBase
    {
        //[Inject] private ICourseService CourseService { get; set; } = null!;

        private int _activeIndex;
        private string? _selectedPl;
        private string? _selectedCt;
        private Guid _selectedCtId;
        private Guid? _selectedModulePreviewId;
        private Guid? _selectedVideoCoursePreviewId;

        //private List<ProgrammingLanguageDtos>? _programmingLanguages;
        //private List<DetailedCourseThemeDtos>? _courseThemes;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            //_programmingLanguages = await CourseService.GetAllProgrammingLanguagesAsync();
            //_courseThemes = await CourseService.AdminGetAllCourseThemesAsync();
        }

        /*private Task<IEnumerable<string>> SearchFromPl(string? value, CancellationToken token)
        {
            if (!string.IsNullOrEmpty(_selectedCt))
                _selectedCt = string.Empty;

            if (_programmingLanguages == null)
                return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());

            var names = _programmingLanguages.Select(x => x.Name);
            return Task.FromResult(string.IsNullOrEmpty(value)
                ? names
                : names.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
        }*/

        /*private Task<IEnumerable<string>> SearchFromCt(string? value, CancellationToken token)
        {
            if (_courseThemes == null)
                return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());

            var filtered = !string.IsNullOrEmpty(_selectedPl)
                ? _courseThemes.Where(x => x.ProgrammingLanguage.Name == _selectedPl)
                : _courseThemes;

            var names = filtered.Select(x => x.Name);
            return Task.FromResult(string.IsNullOrEmpty(value)
                ? names
                : names.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
        }*/

        /*private void UpdateSelectedCtId()
        {
            if (_courseThemes != null && !string.IsNullOrEmpty(_selectedCt))
            {
                var selected = _courseThemes.FirstOrDefault(x => x.Name == _selectedCt);
                if (selected != null)
                    _selectedCtId = selected.Id;
            }
            else if (string.IsNullOrEmpty(_selectedCt))
            {
                _selectedCtId = Guid.Empty;
            }
        }*/

        /*private async Task RefreshDataAsync()
        {
            await LoadDataAsync();
            if (_courseThemes != null)
                _selectedCt = _courseThemes.FirstOrDefault(x => x.Id == _selectedCtId)?.Name;
            StateHasChanged();
        }*/

        private void HandleModuleItemSelected(Guid selectedId)
        {
            _selectedModulePreviewId = selectedId;
            _selectedVideoCoursePreviewId = Guid.Empty;
            StateHasChanged();
        }

        private void HandleVideoCourseItemSelected(Guid selectedId)
        {
            _selectedVideoCoursePreviewId = selectedId;
            StateHasChanged();
        }
    }
}