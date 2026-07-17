using ByteCodePlatform.Admin.Entities.Services.ProjectInfo.Enums;
using ByteCodePlatform.Admin.Entities.Services.ProjectInfo.EventArgs;
using ByteCodePlatform.Admin.Features.admin.course_tree_preview;
using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace ByteCodePlatform.Admin.Widgets.admin_panel.admin_course_themes
{
    public partial class AdminCourseThemes : ComponentBase
    {
        [Inject] private CourseService.Proto.CourseService.CourseServiceClient CourseService { get; set; } = null!;

        ///Переменная для MudTabs (правое меню)
        private int _activeIndex;

        //Переменные для поисковика языков программирования
        private string? _selectedPlText;
        private string? _selectedPlId;

        //Переменные для поисковика тем
        private string? _selectedCtText;
        private Guid _selectedCtId;

        //Кэшированный объект выбранной темы
        private CourseThemeInfoResponse? _selectedCourseTheme;

        //Переменные для определения выбранного модуля/видео в превью
        private Guid _selectedModulePreviewId = Guid.Empty;
        private Guid _selectedVideoCoursePreviewId = Guid.Empty;

        //Массивы данных из бэка
        private List<ProgrammingLanguageInfoResponse>? _programmingLanguages;
        private List<CourseThemeInfoResponse>? _courseThemes;
        private List<CourseModuleInfoResponse>? _courseModules;
        private List<CourseContentInfoResponse>? _courseContents;

        //Свойства
        private CourseThemePropertiesResponse? _courseThemeProperties;
        private CourseModulePropertiesResponse? _courseModuleProperties;
        private CourseContentPropertiesResponse? _courseContentProperties;

        // Для кэша дубликатов имён тем
        private HashSet<string> _duplicateCourseThemeNamesCache = new();
        private CourseTreePreview? _courseTreePreviewRef;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadInitialDataAsync();
        }

        private async Task LoadInitialDataAsync()
        {
            var plRequest = CourseService
                .GetProgrammingLanguagesAsync(new GetProgrammingLanguagesRequest()).ResponseAsync;
            var ctRequest = CourseService
                .GetCourseThemesAsync(new GetCourseThemesRequest { IgnoreFilters = true }).ResponseAsync;

            await Task.WhenAll(plRequest, ctRequest);

            _programmingLanguages = plRequest.Result.ProgrammingLanguages.ToList();
            _courseThemes = ctRequest.Result.CourseThemes.ToList();

            RebuildDuplicateNamesCache();
        }

        /// <summary>
        /// Обновление списка тем
        /// </summary>
        private async Task RefreshCourseThemesAsync()
        {
            var response = await CourseService.GetCourseThemesAsync(
                new GetCourseThemesRequest { IgnoreFilters = true });

            _courseThemes = response.CourseThemes.ToList();

            RebuildDuplicateNamesCache();
        }

        private void RebuildDuplicateNamesCache()
        {
            if (_courseThemes == null)
            {
                _duplicateCourseThemeNamesCache = new();
                return;
            }

            var filtered = !string.IsNullOrEmpty(_selectedPlId)
                ? _courseThemes.Where(x => x.ProgrammingLanguage.Id == _selectedPlId)
                : _courseThemes;

            _duplicateCourseThemeNamesCache = filtered
                .GroupBy(x => x.Name)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet();
        }

        /// <summary>
        /// Поиск языка программирования
        /// </summary>
        private Task<IEnumerable<string>> SearchFromPl(string? value, CancellationToken token)
        {
            if (_programmingLanguages == null)
                return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());

            var names = _programmingLanguages.Select(x => x.Name);
            return Task.FromResult(string.IsNullOrEmpty(value)
                ? names
                : names.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        /// Логика изменения языка программирования
        /// </summary>
        private Task OnPlChanged()
        {
            _selectedCtText = string.Empty;
            _selectedCtId = Guid.Empty;
            _selectedCourseTheme = null;
            _courseThemeProperties = null;
            _courseModules = null;

            _selectedPlId = !string.IsNullOrEmpty(_selectedPlText)
                ? _programmingLanguages?.FirstOrDefault(x => x.Name == _selectedPlText)?.Id
                : null;

            RebuildDuplicateNamesCache();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Поиск темы курса
        /// </summary>
        private Task<IEnumerable<string>> SearchFromCt(string? value, CancellationToken token)
        {
            if (_courseThemes == null)
                return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());

            var filtered = !string.IsNullOrEmpty(_selectedPlId)
                ? _courseThemes.Where(x => x.ProgrammingLanguage.Id == _selectedPlId)
                : _courseThemes;

            var toDisplayNames = filtered.Select(x =>
                _duplicateCourseThemeNamesCache.Contains(x.Name)
                    ? $"{x.Name} ({ProgramLanguagesEnum.FromId(Guid.Parse(x.ProgrammingLanguage.Id))})"
                    : x.Name);

            return Task.FromResult(string.IsNullOrEmpty(value)
                ? toDisplayNames
                : toDisplayNames.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
        }

        private async Task UpdateSelectedCtId()
        {
            if (_courseThemes != null && !string.IsNullOrEmpty(_selectedCtText))
            {
                var (themeName, languageName) = ParseDisplayName(_selectedCtText);
                var programmingLanguageId = !string.IsNullOrEmpty(languageName) ? languageName : _selectedPlId;

                var selectedCt = string.IsNullOrEmpty(programmingLanguageId)
                    ? _courseThemes.FirstOrDefault(x => x.Name == themeName)
                    : _courseThemes.FirstOrDefault(x =>
                        x.Name == themeName &&
                        x.ProgrammingLanguage.Id == programmingLanguageId);

                if (selectedCt != null && Guid.TryParse(selectedCt.Id, out var courseThemeId))
                {
                    _selectedCtId = courseThemeId;
                    _selectedCourseTheme = selectedCt;
                    _selectedModulePreviewId = Guid.Empty;
                    _selectedVideoCoursePreviewId = Guid.Empty;
                    await UpdateProperties();
                }
            }
            else
            {
                _selectedCtId = Guid.Empty;
                _selectedCourseTheme = null;
                _courseThemeProperties = null;
                _courseModules = null;
            }
        }

        private static (string CourseThemeName, string ProgrammingLanguageName) ParseDisplayName(string selectedCtName)
        {
            var lastParentIndex = selectedCtName.LastIndexOf(" (", StringComparison.Ordinal);

            if (lastParentIndex <= 0)
                return (selectedCtName, string.Empty);

            var courseThemeName = selectedCtName.Substring(0, lastParentIndex);
            var programmingLanguageName = selectedCtName.Substring(lastParentIndex + 2).TrimEnd(')');
            programmingLanguageName = ProgramLanguagesEnum.FromName(programmingLanguageName).Id.ToString();

            return (courseThemeName, programmingLanguageName);
        }

        private async Task RefreshDataAsync()
        {
            await RefreshCourseThemesAsync();

            if (_courseThemes != null && _selectedCtId != Guid.Empty)
            {
                _selectedCourseTheme = _courseThemes.FirstOrDefault(x =>
                    Guid.TryParse(x.Id, out var id) && id == _selectedCtId);
                _selectedCtText = _selectedCourseTheme?.Name;
            }

            await UpdateProperties();
            /*Для очистки кэша видео контента*/
            if (_courseTreePreviewRef != null)
            {
                _courseContents = await _courseTreePreviewRef.RefreshExpandedModuleAsync();
            }
        }

        /// <summary>
        /// Логика добавления новой темы
        /// </summary>
        private async Task HandleCourseThemeCreated(Guid newCourseThemeId)
        {
            _selectedModulePreviewId = Guid.Empty;
            _selectedVideoCoursePreviewId = Guid.Empty;
            _selectedCtId = newCourseThemeId;
            
            await RefreshDataAsync();
        }

        /// <summary>
        /// Логика выбора модуля
        /// </summary>
        private async Task HandleModuleItemSelected(Guid selectedId)
        {
            _selectedModulePreviewId = selectedId;
            _selectedVideoCoursePreviewId = Guid.Empty;
            _courseModuleProperties = await CourseService.GetCourseModulePropertiesAsync(
                new GetCourseModulePropertiesRequest
                {
                    CourseModuleId = _selectedModulePreviewId.ToString()
                });
        }

        /// <summary>
        /// Логика выбора контента (видео)
        /// </summary>
        private async Task HandleVideoCourseItemSelected(VideoCourseSelectedEventArgs contentSelected)
        {
            _selectedVideoCoursePreviewId = contentSelected.CourseId;
            _courseContents = contentSelected.CourseContents;
            _courseContentProperties = await CourseService.GetCourseContentPropertiesAsync(
                new GetCourseContentPropertiesRequest
                {
                    CourseContentId = _selectedVideoCoursePreviewId.ToString()
                });
        }

        /// <summary>
        /// Логика обновления свойств
        /// </summary>
        private async Task UpdateProperties()
        {
            if (_selectedCtId == Guid.Empty) return;

            var courseThemeRequest = CourseService.GetCourseThemePropertiesAsync(
                new GetCourseThemePropertiesRequest
                {
                    CourseThemeId = _selectedCtId.ToString()
                }
            ).ResponseAsync;

            var courseModulesRequest = CourseService.GetCourseModulesByCourseIdAsync(
                new GetCourseModulesByCourseIdRequest
                {
                    CourseId = _selectedCtId.ToString(),
                    IgnoreFilters = true
                }
            ).ResponseAsync;

            await Task.WhenAll(courseThemeRequest, courseModulesRequest);

            _courseThemeProperties = courseThemeRequest.Result;
            _courseModules = courseModulesRequest.Result.CourseModules.ToList();

            if (_selectedModulePreviewId != Guid.Empty)
            {
                _courseModuleProperties = await CourseService.GetCourseModulePropertiesAsync(
                    new GetCourseModulePropertiesRequest
                    {
                        CourseModuleId = _selectedModulePreviewId.ToString()
                    });
            }

            if (_selectedVideoCoursePreviewId != Guid.Empty)
            {
                _courseContentProperties = await CourseService.GetCourseContentPropertiesAsync(
                    new GetCourseContentPropertiesRequest
                    {
                        CourseContentId = _selectedVideoCoursePreviewId.ToString()
                    });
            }
        }

        /// <summary>
        /// Логика добавления нового модуля
        /// </summary>
        private async Task AddNewCourseModule()
        {
            var newCourseModuleResponse = await CourseService.AddCourseModuleAsync(new AddCourseModuleRequest
            {
                Name = $"Новый модуль {_courseModules?.Count + 1}",
                CourseThemeId = _selectedCtId.ToString(),
            });

            if (newCourseModuleResponse.Success) await UpdateProperties();
        }

        private async Task AddNewCourseContent()
        {
            var newCourseContentResponse = await CourseService.AddCourseContentAsync(new AddCourseContentRequest
            {
                Name = $"Новый видеокурс {_courseContents?.Count + 1}",
                CourseModuleId =  _selectedModulePreviewId.ToString(),
                ImgUrl = "Укажите ссылку на изображение",
                LinkOnRutube = "Укажите ссылку на RuTube",
                LinkOnVk = "Укажите ссылку на Vk Видео",
                LinkOnYoutube = "Укажите ссылку на YouTube"
            });

            if (newCourseContentResponse.Success) await UpdateProperties();

            /*Для очистки кэша видео контента*/
            if (_courseTreePreviewRef != null)
            {
                _courseContents = await _courseTreePreviewRef.RefreshExpandedModuleAsync();
            }
        }
    }
}