namespace ContractualDtos.DTO.User.UserProfile.Requests
{
    public record UpdateUserProfileRequestDto(
        Guid Id,
        string Surname,
        string Name,
        string Patronymic,
        string Email,
        string AltName,
        string Password);
}