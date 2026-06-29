using Microsoft.AspNetCore.Components;

namespace ByteCodePlatform.Admin.Features.admin.course_tree_preview
{
    public partial class CourseTreePreview : ComponentBase
    {
        //[Parameter] public IEnumerable<DetailedCourseThemeDtos>? CourseTheme { get; set; }
        [Parameter] public string? SelectedCt { get; set; }

        [Parameter] public EventCallback<Guid> OnCourseModuleSelected { get; set; }
        [Parameter] public EventCallback<Guid> OnVideoCourseSelected { get; set; }

        private Guid? _expandedModuleId;
        private Guid? _expandedVideoCourseId;
        private Guid _selectedCmPreviewId;
        private Guid _selectedVcPreviewId;

        private async Task OnModuleExpandedChanged(Guid moduleId, bool expanded)
        {
            if (expanded)
            {
                _expandedModuleId = moduleId;
                _expandedVideoCourseId = null;
                _selectedCmPreviewId = moduleId;
            }
            else
            {
                _expandedModuleId = null;
                _selectedCmPreviewId = Guid.Empty;
            }

            if (OnCourseModuleSelected.HasDelegate)
                await OnCourseModuleSelected.InvokeAsync(_selectedCmPreviewId);
        }

        private bool GetModuleExpandedState(Guid moduleId) => _expandedModuleId == moduleId;

        private async Task OnVideoCourseExpandedChanged(Guid videoId, bool expanded)
        {
            if (expanded)
            {
                _expandedVideoCourseId = videoId;
                _selectedVcPreviewId = videoId;
            }
            else
            {
                _expandedVideoCourseId = null;
                _selectedVcPreviewId = Guid.Empty;
            }

            if (OnVideoCourseSelected.HasDelegate)
                await OnVideoCourseSelected.InvokeAsync(_selectedVcPreviewId);
        }

        private bool GetVideoCourseExpandedState(Guid videoId) => _expandedVideoCourseId == videoId;
    }
}