namespace ContractualDtos.DTO.Course.CourseAuthor.Dtos;

/// <summary>
/// DTO для возврата объекта
/// </summary>
public record CourseAuthorDtos(
    Guid Id,
    string Surname,
    string Name,
    string Patronymic,
    string NickName)
{
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
    /// Альтернативное имя
    /// </summary>
    public string NickName { get; init; } = NickName;
}