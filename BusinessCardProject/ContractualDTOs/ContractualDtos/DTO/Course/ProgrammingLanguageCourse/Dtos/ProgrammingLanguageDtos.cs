namespace ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;

/// <summary>
/// DTO для возврата объекта
/// </summary>
public record ProgrammingLanguageDtos
{
    public ProgrammingLanguageDtos(Guid id, string name, int countSelectedUser)
    {
        Id = id;
        Name = name;
        СountSelectedUser = countSelectedUser;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название ЯП
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Кол-во пользователей выбравших этот ЯП
    /// </summary>
    public int СountSelectedUser { get; init; }
}