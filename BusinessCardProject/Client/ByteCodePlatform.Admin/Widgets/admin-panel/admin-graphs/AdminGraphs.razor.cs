using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ByteCodePlatform.Admin.Widgets.admin_panel.admin_graphs
{
    public partial class AdminGraphs : ComponentBase
    {
        private int _index = -1;
        private ChartOptions _options = new();
        private AxisChartOptions _axisChartOptions = new();

        private List<ChartSeries> _series = new()
        {
            new()
            {
                Name = "Прошлая",
                Data = new double[] { 90, 79, 72, 69, 62, 62, 55 },
                ShowDataMarkers = true
            },
            new()
            {
                Name = "Текущая",
                Data = new double[] { 10, 41, 35, 51, 49, 62, 69 },
                ShowDataMarkers = true
            }
        };

        private string[] _xAxisLabels = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _axisChartOptions.MatchBoundsToSize = true;
            _series.ForEach(x => x.ShowDataMarkers = true);
            _options.ChartPalette = new[] { "#e74c3c", "#2ecc71" };
        }
    }
}