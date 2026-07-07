namespace Requests.User
{
    public record GetUsersSearchRequest(
        int Page,
        int PageSize,
        string Search,
        string SortBy,
        string SortDirection
    );
}