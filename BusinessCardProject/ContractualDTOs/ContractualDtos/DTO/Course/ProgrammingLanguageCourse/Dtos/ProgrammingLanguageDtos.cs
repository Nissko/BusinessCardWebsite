namespace ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;

/// <summary>
/// DTO для возврата объекта
/// </summary>
public record ProgrammingLanguageDtos(
    Guid Id,
    string Name,
    int CountSelectedUser);