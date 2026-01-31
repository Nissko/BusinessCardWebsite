using ContractualDtos.DTO.Course.CourseAuthor.Dtos;
using ContractualDtos.DTO.Course.CourseAuthor.Requests;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;

public interface ICourseAuthorRepository
{
    /// <summary>
    /// Вывод всех авторов
    /// </summary>
    Task<List<CourseAuthorDtos>> GetAllAsync();

    /// <summary>
    /// Добавление нового автора
    /// </summary>
    /// <param name="dto">передаваемые параметры из запроса</param>
    Task<bool?> Create(CreateCourseAuthorRequestDto dto);

    /// <summary>
    /// Поиск определенного автора
    /// </summary>
    /// <param name="id">передаваемые параметры из запроса</param>
    Task<CourseAuthorDtos?> Read(Guid id);

    /// <summary>
    /// Изменение определенного автора
    /// </summary>
    /// <param name="dto">передаваемые параметры из запроса</param>
    Task<CourseAuthorDtos?> Update(UpdateCourseAuthorRequestDto dto);

    /// <summary>
    /// Удаление определенного автора
    /// </summary>
    /// <param name="id">передаваемые параметры из запроса</param>
    Task<bool> Delete(Guid id);
}