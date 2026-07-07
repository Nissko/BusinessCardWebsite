using Microsoft.AspNetCore.Components;
using MudBlazor;
using UserService.Proto;

namespace ByteCodePlatform.Admin.Widgets.admin_panel.admin_users
{
    public partial class AdminUsers : ComponentBase
    {
        [Inject] private UserGrpcService.UserGrpcServiceClient UserService { get; set; } = null!;

        private MudTable<UserInfoResponse>? _table;
        private bool _isLoading = true;
        private string _searchString = "";

        private async Task<TableData<UserInfoResponse>> ServerReload(TableState state, CancellationToken token)
        {
            _isLoading = true;
            StateHasChanged();

            try
            {
                var sortDirection = (int)state.SortDirection;
                var response = await UserService.GetUsersFromSearchAsync(new GetUsersFromSearchRequest
                {
                    Page = state.Page + 1,
                    PageSize = state.PageSize,
                    Search = _searchString,
                    SortBy = state.SortLabel ?? "",
                    SortDirection = sortDirection switch
                    {
                        0 => "none",
                        1 => "asc",
                        _ => "desc"
                    }
                });

                if (response is null)
                {
                    return new TableData<UserInfoResponse>
                    {
                        TotalItems = 0,
                        Items = []
                    };
                }

                return new TableData<UserInfoResponse>
                {
                    TotalItems = response.TotalCount,
                    Items = response.Items.ToList()
                };
            }
            catch
            {
                return new TableData<UserInfoResponse>
                {
                    TotalItems = 0,
                    Items = []
                };
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table?.ReloadServerData();
        }
    }
}