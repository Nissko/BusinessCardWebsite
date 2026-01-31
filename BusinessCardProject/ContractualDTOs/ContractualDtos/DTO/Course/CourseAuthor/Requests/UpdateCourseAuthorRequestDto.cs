namespace ContractualDtos.DTO.Course.CourseAuthor.Requests;

public record UpdateCourseAuthorRequestDto(
    Guid Id,
    string Surname,
    string Name,
    string Patronymic,
    string NickName);