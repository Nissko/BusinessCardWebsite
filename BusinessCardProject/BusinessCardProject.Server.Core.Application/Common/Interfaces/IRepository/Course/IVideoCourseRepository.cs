using ContractualDtos.DTO.Course.VideoCourse.Dtos;
using ContractualDtos.DTO.Course.VideoCourse.Requests;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;

public interface IVideoCourseRepository
{
    /// <summary>
    /// Вывод всех модулей
    /// </summary>
    Task<List<VideoCourseDtos>> GetAllAsync();

    /// <summary>
    /// Добавление нового модуля
    /// </summary>
    /// <param name="dto">передаваемые параметры из запроса</param>
    Task<bool?> Create(CreateVideoCourseRequestDto dto);

    /// <summary>
    /// Поиск определенного модуля
    /// </summary>
    /// <param name="id">передаваемые параметры из запроса</param>
    Task<VideoCourseDtos?> Read(Guid id);

    /// <summary>
    /// Изменение определенного модуля
    /// </summary>
    /// <param name="dto">передаваемые параметры из запроса</param>
    Task<VideoCourseDtos?> Update(UpdateVideoCourseRequestDto dto);

    /// <summary>
    /// Удаление определенного модуля
    /// </summary>
    /// <param name="id">передаваемые параметры из запроса</param>
    Task<bool> Delete(Guid id);
}