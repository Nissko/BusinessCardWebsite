using System.ComponentModel.DataAnnotations;

namespace Requests.User
{
    public class CreateUserRequest
    {
        [Required] public string Surname { get; init; } = string.Empty;
        [Required] public string Name { get; init; } = string.Empty;
        [Required] public string NickName { get; init; } = string.Empty;
        [Required, EmailAddress] public string Email { get; init; } = string.Empty;
        [Required, MinLength(6)] public string Password { get; init; } = string.Empty;
    }
}