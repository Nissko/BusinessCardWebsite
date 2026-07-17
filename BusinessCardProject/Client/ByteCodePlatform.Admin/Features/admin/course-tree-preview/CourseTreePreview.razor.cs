using ByteCodePlatform.Admin.Entities.Services.ProjectInfo.EventArgs;
using CourseService.Proto;
using Microsoft.AspNetCore.Components;

namespace ByteCodePlatform.Admin.Features.admin.course_tree_preview
{
    public partial class CourseTreePreview : ComponentBase
    {
        [Inject] private CourseService.Proto.CourseService.CourseServiceClient CourseService { get; set; } = null!;

        /// <summary>
        /// Темы с бэка
        /// </summary>
        [Parameter]
        public CourseThemeInfoResponse? CourseTheme { get; set; }

        /// <summary>
        /// Модули с бэка
        /// </summary>
        [Parameter]
        public List<CourseModuleInfoResponse>? CourseModules { get; set; }

        /// <summary>
        /// Идентификатор выбранной темы курса
        /// </summary>
        [Parameter]
        public Guid? SelectedCtId { get; set; }

        /// <summary>
        /// Наименование выбранного ЯП
        /// </summary>
        [Parameter]
        public string? SelectedPl { get; set; }

        /// <summary>
        /// Идентификатор выбранного ЯП
        /// </summary>
        [Parameter]
        public string? SelectedPlId { get; set; }

        /// <summary>
        /// Событие при выборе модуля курса
        /// </summary>
        [Parameter]
        public EventCallback<Guid> OnCourseModuleSelected { get; set; }

        /// <summary>
        /// Событие при выборе контента курса
        /// </summary>
        [Parameter]
        public EventCallback<VideoCourseSelectedEventArgs> OnVideoCourseSelected { get; set; }

        /// <summary>
        /// Событие при создании темы курса
        /// </summary>
        [Parameter]
        public EventCallback<Guid> OnCourseThemeCreated { get; set; }

        private Guid _expandedModuleId = Guid.Empty;
        private Guid _expandedVideoCourseId = Guid.Empty;
        private Guid _selectedCmPreviewId = Guid.Empty;
        private Guid _selectedVcPreviewId = Guid.Empty;
        private readonly Dictionary<Guid, List<CourseContentInfoResponse>> _moduleContentsCache = new();

        protected override void OnParametersSet()
        {
            if (CourseTheme == null ||
                (SelectedCtId.HasValue && !_moduleContentsCache.Keys.Any()))
            {
                ClearCache();
            }
        }

        public void ClearCache()
        {
            _moduleContentsCache.Clear();
            _expandedModuleId = Guid.Empty;
            _expandedVideoCourseId = Guid.Empty;
        }
        
        public async Task<List<CourseContentInfoResponse>> RefreshExpandedModuleAsync()
        {
            if (_expandedModuleId != Guid.Empty && CourseModules?.Any(m => m.Id == _expandedModuleId.ToString()) == true)
            {
                var response = await CourseService.GetCourseContentsByModuleIdAsync(
                    new GetCourseContentsByModuleIdRequest
                    {
                        ModuleId = _expandedModuleId.ToString(),
                        IgnoreFilters = true
                    });
                _moduleContentsCache[_expandedModuleId] = response.CourseContents.ToList();
                
                return response.CourseContents.ToList();
            }

            return new List<CourseContentInfoResponse>();
        }

        private bool GetModuleExpandedState(Guid moduleId) => _expandedModuleId == moduleId;
        private bool GetVideoCourseExpandedState(Guid videoId) => _expandedVideoCourseId == videoId;

        private async Task OnModuleExpandedChanged(Guid moduleId, bool isExpanded)
        {
            if (isExpanded)
            {
                _expandedModuleId = moduleId;
                _expandedVideoCourseId = Guid.Empty;
                _selectedCmPreviewId = moduleId;

                if (!_moduleContentsCache.TryGetValue(moduleId, out var courseContents))
                {
                    var response = await CourseService.GetCourseContentsByModuleIdAsync(
                        new GetCourseContentsByModuleIdRequest
                        {
                            ModuleId = moduleId.ToString(),
                            IgnoreFilters = true
                        });
                    courseContents = response.CourseContents.ToList();
                    _moduleContentsCache[moduleId] = courseContents;
                }
            }
            else if (_expandedModuleId == moduleId)
            {
                _expandedModuleId = Guid.Empty;
                _selectedCmPreviewId = Guid.Empty;
            }

            if (OnCourseModuleSelected.HasDelegate)
                await OnCourseModuleSelected.InvokeAsync(_selectedCmPreviewId);
        }

        private async Task OnVideoCourseExpandedChanged(Guid videoId, bool isExpanded)
        {
            if (isExpanded)
            {
                _expandedVideoCourseId = videoId;
                _selectedVcPreviewId = videoId;
            }
            else if (_expandedVideoCourseId == videoId)
            {
                _expandedVideoCourseId = Guid.Empty;
                _selectedVcPreviewId = Guid.Empty;
            }

            if (OnVideoCourseSelected.HasDelegate)
            {
                await OnVideoCourseSelected.InvokeAsync(new VideoCourseSelectedEventArgs
                {
                    CourseId = _selectedVcPreviewId, 
                    CourseContents = _moduleContentsCache[_expandedModuleId]
                });
            }
        }

        private async Task AddNewCourseTheme()
        {
            /*TODO: Исправить хардкод (authorId + сделать проверку на дубликат имен в рамках ЯП)*/
            var newCourseThemeResponse = await CourseService.AddCourseThemeAsync(new AddCourseThemeRequest
            {
                AuthorId = "1cb3fda6-4296-4d95-bc7d-dd23e0b5e34b",
                AvatarUrl = "null",
                Description = "Введите описание",
                Name = Guid.NewGuid().ToString(),
                OldPrice = 2,
                Price = 1,
                ProgrammingLanguageId = SelectedPlId
            });

            if (OnCourseThemeCreated.HasDelegate && Guid.TryParse(newCourseThemeResponse.Id, out var newId))
            {
                await OnCourseThemeCreated.InvokeAsync(newId);
            }
        }
    }
}