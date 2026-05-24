namespace Requests.User
{
    public record CreateUserRequest(string Surname, string Name, string NickName, string Email, string Password);
}