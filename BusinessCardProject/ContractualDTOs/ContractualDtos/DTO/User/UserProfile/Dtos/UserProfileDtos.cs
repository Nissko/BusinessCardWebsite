using NodaTime;

namespace ContractualDtos.DTO.User.UserProfile.Dtos;

/// <summary>
/// DTO для возврата объекта
/// </summary>
public record UserProfileDtos(
    Guid Id,
    string Surname,
    string Name,
    string Patronymic,
    string Email,
    string AltName,
    string CreatedOn)
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get; init; } = Surname;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; init; } = Name;

    /// <summary>
    /// Отчество
    /// </summary>
    public string Patronymic { get; init; } = Patronymic;

    /// <summary>
    /// Почта
    /// </summary>
    public string Email { get; init; } = Email;

    /// <summary>
    /// Никнейм
    /// </summary>
    public string AltName { get; init; } = AltName;
    
    /// <summary>
    /// Дата создания профиля
    /// </summary>
    public string CreatedOn { get; init; } = CreatedOn;
}