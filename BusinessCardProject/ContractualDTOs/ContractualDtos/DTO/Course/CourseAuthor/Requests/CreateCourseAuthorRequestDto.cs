namespace ContractualDtos.DTO.Course.CourseAuthor.Requests
{
    /// <summary>
    /// DTO для запросов
    /// </summary>
    public record CreateCourseAuthorRequestDto(
        string Surname,
        string Name,
        string Patronymic,
        string NickName);
}