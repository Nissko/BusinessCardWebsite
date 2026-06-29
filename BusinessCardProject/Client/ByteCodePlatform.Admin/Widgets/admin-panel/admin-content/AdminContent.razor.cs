using ByteCodePlatform.Admin.Shared.config.admin;
using Microsoft.AspNetCore.Components;

namespace ByteCodePlatform.Admin.Widgets.admin_panel.admin_content
{
    public partial class AdminContent : ComponentBase
    {
        [Parameter, EditorRequired]
        public AdminFiltersBtns Section { get; set; }
    }
}