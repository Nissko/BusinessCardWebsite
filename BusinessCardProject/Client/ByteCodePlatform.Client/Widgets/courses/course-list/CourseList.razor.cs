using CourseService.Proto;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BusinessCardProject.Client.Widgets.courses.course_list;

public partial class CourseList : ComponentBase, IDisposable
{
    [Inject] private CourseService.Proto.CourseService.CourseServiceClient CourseGrpc { get; set; } = null!;
    [Inject] private IBrowserViewportService BrowserViewportService { get; set; } = null!;
    [Inject] private NavigationManager NavManager { get; set; } = null!;

    [Parameter] public EventCallback<Guid> OnCourseSelectedEvent { get; set; }

    private List<CourseThemeInfoResponse>? _courses;
    private int _skeletonCount = 6;
    private Guid _viewportSubscriptionId;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _viewportSubscriptionId = Guid.NewGuid();
        await BrowserViewportService.SubscribeAsync(
            _viewportSubscriptionId,
            OnViewportChanged,
            fireImmediately: true);

        _courses = await LoadCoursesAsync();

        if (_courses.Any())
        {
            await BrowserViewportService.UnsubscribeAsync(_viewportSubscriptionId);
        }
    }

    private async Task<List<CourseThemeInfoResponse>> LoadCoursesAsync()
    {
        var request = new GetCourseThemesRequest();
        var result = await CourseGrpc.GetCourseThemesAsync(request);
        return result.CourseThemes.ToList();
    }

    private async Task OnViewportChanged(BrowserViewportEventArgs args)
    {
        if (_courses != null) return;

        _skeletonCount = CalculateSkeletonCount(args.Breakpoint);
        await InvokeAsync(StateHasChanged);
    }

    private static int CalculateSkeletonCount(Breakpoint breakpoint)
    {
        return breakpoint switch
        {
            Breakpoint.Xxl => 9,
            Breakpoint.Xl or Breakpoint.Lg or Breakpoint.Md => 6,
            Breakpoint.Sm => 4,
            Breakpoint.Xs => 2,
            _ => 6
        };
    }

    private void OnCourseSelected(Guid themeId)
    {
        NavManager.NavigateTo($"/course/{themeId}");
    }

    public void Dispose()
    {
        BrowserViewportService.UnsubscribeAsync(_viewportSubscriptionId).ConfigureAwait(false);
    }
}