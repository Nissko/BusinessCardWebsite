using ContractualDtos.DTO.Course.CourseModule.Dtos;
using ContractualDtos.DTO.Course.CourseModule.Requests;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course
{
    public interface ICourseModuleRepository
    {
        /// <summary>
        /// Вывод всех модулей
        /// </summary>
        Task<List<DetailedCourseModuleDtos>> GetAllAsync();

        /// <summary>
        /// Добавление нового модуля
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<bool?> Create(CreateCourseModuleRequestDto dto);

        /// <summary>
        /// Поиск определенного модуля
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<DetailedCourseModuleDtos?> Read(Guid id);

        /// <summary>
        /// Изменение определенного модуля
        /// </summary>
        /// <param name="dto">передаваемые параметры из запроса</param>
        Task<bool> Update(UpdateCourseModuleRequestDto dto);

        /// <summary>
        /// Удаление определенного модуля
        /// </summary>
        /// <param name="id">передаваемые параметры из запроса</param>
        Task<bool> Delete(Guid id);
    }
}