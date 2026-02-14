using ContractualDtos.DTO.Course.CourseTheme.Dtos;
using ContractualDtos.DTO.Course.CourseTheme.Requests;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course
{
    public interface ICourseThemeRepository
    {
        /// <summary>
        /// Вывод всех тем
        /// </summary>
        Task<List<DetailedCourseThemeDtos>> GetAllAsync();

        /// <summary>
        /// Добавление новой темы
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<bool?> Create(CreateCourseThemeRequestDto dto);

        /// <summary>
        /// Поиск определенной темы
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<DetailedCourseThemeDtos?> Read(Guid id);

        /// <summary>
        /// Изменение определенной темы
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<DetailedCourseThemeDtos?> Update(UpdateCourseThemeRequestDto dto);

        /// <summary>
        /// Удаление определенной темы
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<bool> Delete(Guid id);
    }
}