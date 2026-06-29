namespace Requests.User
{
    public record UpdateUserRequest(
        Guid Id,
        string Surname,
        string Name,
        string NickName,
        string Email);
}