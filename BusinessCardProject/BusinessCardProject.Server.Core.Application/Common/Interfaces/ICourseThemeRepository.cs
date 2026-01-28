using ContractualDtos.DTO.CourseTheme.Dtos;
using ContractualDtos.DTO.CourseTheme.Requests;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces;

public interface ICourseThemeRepository
{
    /// <summary>
    /// Вывод всех тем
    /// </summary>
    Task<List<CourseThemeDtos>> GetAllAsync();

    /// <summary>
    /// Добавление новой темы
    /// </summary>
    /// <param name="dto">передаваемые параметры из запроса</param>
    Task<CourseThemeDtos> Create(CreateCourseThemeRequestDto dto);

    /// <summary>
    /// Поиск определенной темы
    /// </summary>
    /// <param name="id">передаваемые параметры из запроса</param>
    Task<CourseThemeDtos?> Read(Guid id);

    /// <summary>
    /// Изменение определенной темы
    /// </summary>
    /// <param name="programmingLanguage">передаваемые параметры из запроса</param>
    Task<CourseThemeDtos?> Update(UpdateCourseThemeRequestDto dto);

    /// <summary>
    /// Удаление определенной темы
    /// </summary>
    /// <param name="id">передаваемые параметры из запроса</param>
    Task<bool> Delete(Guid id);
}