using CourseService.Proto;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BusinessCardProject.Client.Entities.Enums;

namespace BusinessCardProject.Client.Widgets.courses.course_list
{
    public partial class CourseList : ComponentBase, IDisposable
    {
        [Inject]
        private CourseService.Proto.CourseService.CourseServiceClient CourseGrpcServiceClient { get; set; } = null!;

        [Inject] private IBrowserViewportService BrowserViewportService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;

        [Parameter] public EventCallback<Guid> OnCourseSelectedEvent { get; set; }

        private List<CourseThemeInfoResponse>? _courses;
        private readonly List<CourseThemePropertiesResponse>? _courseThemeProperties = new();
        private int _skeletonCount = 9;
        private Guid _viewportSubscriptionId;
        private bool _disposed;

        private readonly HashSet<Guid> _selectedLanguageIds = new();
        private CostFilter _selectedCostFilter = CostFilter.All;
        private bool _filtersOpen;

        private int _loadedCount = 10;
        private const int PageSize = 10;

        private bool HasActiveFilters => _selectedLanguageIds.Any() || _selectedCostFilter != CostFilter.All;

        /// <summary>
        /// Полный отфильтрованный список курсов (без ограничения Take)
        /// </summary>
        private List<CourseThemeInfoResponse>? AllFilteredCourses
        {
            get
            {
                if (_courses == null) return null;

                var query = _courses.AsEnumerable();

                if (_selectedLanguageIds.Any())
                {
                    query = query.Where(c => _selectedLanguageIds.Any(id => id.ToString() == c.ProgrammingLanguage.Id));
                }

                query = _selectedCostFilter switch
                {
                    CostFilter.Free => query.Where(c => c.IsFree),
                    CostFilter.Paid => query.Where(c => !c.IsFree),
                    _ => query
                };

                return query.ToList();
            }
        }

        /// <summary>
        /// Отфильтрованный список курсов для отображения (с ограничением Take)
        /// </summary>
        private List<CourseThemeInfoResponse>? FilteredCourses
            => AllFilteredCourses?.Take(_loadedCount).ToList();

        /// <summary>
        /// Есть ли ещё курсы для загрузки
        /// </summary>
        private bool HasMoreCourses => (AllFilteredCourses?.Count ?? 0) > _loadedCount;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (_disposed) return;

            _viewportSubscriptionId = Guid.NewGuid();

            await BrowserViewportService.SubscribeAsync(
                _viewportSubscriptionId,
                OnViewportChanged,
                fireImmediately: true);

            _courses = await LoadCoursesAsync();

            if (_courses != null && _courses.Any())
            {
                await LoadCourseThemePropertiesAsync();
            }
        }

        /// <summary>
        /// Добавление темы курса в список для поиска
        /// </summary>
        private void ToggleLanguage(Guid id, bool selected)
        {
            if (selected)
            {
                _selectedLanguageIds.Add(id);
            }
            else
            {
                _selectedLanguageIds.Remove(id);
            }

            _loadedCount = PageSize;
        }

        private void ClearFilters()
        {
            _selectedLanguageIds.Clear();
            _selectedCostFilter = CostFilter.All;
            _loadedCount = PageSize;
        }
        
        private void LoadMoreCourses()
        {
            _loadedCount += PageSize;
        }

        /// <summary>
        /// Получение всех курсов
        /// </summary>
        private async Task<List<CourseThemeInfoResponse>> LoadCoursesAsync()
        {
            var response = await CourseGrpcServiceClient.GetCourseThemesAsync(new GetCourseThemesRequest());
            return response.CourseThemes.ToList();
        }

        /// <summary>
        /// Получение свойств тем курса
        /// </summary>
        private async Task LoadCourseThemePropertiesAsync()
        {
            if (_courses != null)
            {
                foreach (var request in _courses.Select(course => new GetCourseThemePropertiesRequest
                         {
                             CourseThemeId = course.Id
                         }))
                {
                    var response = await CourseGrpcServiceClient.GetCourseThemePropertiesAsync(request);
                    _courseThemeProperties?.Add(response);
                }
            }
        }

        /// <summary>
        /// Действия выполняемы при изменении масштаба экрана
        /// </summary>
        private async Task OnViewportChanged(BrowserViewportEventArgs args)
        {
            if (_courses != null) return;

            _skeletonCount = CalculateSkeletonCount(args.Breakpoint);
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Шаблон скелетонов в зависимости от ширины экрана устройства
        /// </summary>
        private static int CalculateSkeletonCount(Breakpoint breakpoint)
        {
            return breakpoint switch
            {
                Breakpoint.Xxl => 12,
                Breakpoint.Xl or Breakpoint.Lg or Breakpoint.Md => 9,
                Breakpoint.Sm => 6,
                Breakpoint.Xs => 4,
                _ => 9
            };
        }

        private void OnCourseSelected(Guid themeId)
        {
            NavManager.NavigateTo($"/course/{themeId}");
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                BrowserViewportService.UnsubscribeAsync(_viewportSubscriptionId).ConfigureAwait(false);
            }
            catch
            {
                // ignored
            }

            GC.SuppressFinalize(this);
        }
    }
}