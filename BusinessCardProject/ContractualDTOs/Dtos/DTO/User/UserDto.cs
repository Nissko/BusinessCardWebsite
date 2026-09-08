using NodaTime;

namespace DTOs.DTO.User
{
    public record UserDto(
        Guid Id,
        string Surname,
        string Name,
        string NickName,
        string Email,
        bool IsAuthor,
        Instant CreatedAt,
        Instant? UpdatedAt,
        Instant? DeletedAt,
        bool IsVerified,
        string UserAvatar);
}