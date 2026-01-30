namespace ContractualDtos.DTO.User.UserProfile.Requests;

/// <summary>
/// DTO для запросов
/// </summary>
public record CreateUserProfileRequestDto(
    string Surname,
    string Name,
    string Patronymic,
    string Email,
    string AltName,
    string Password);