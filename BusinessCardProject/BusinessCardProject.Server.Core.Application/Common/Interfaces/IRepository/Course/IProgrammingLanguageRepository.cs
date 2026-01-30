using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;
using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Requests;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;

public interface IProgrammingLanguageRepository
{
    /// <summary>
    /// Вывод всех доступных языков
    /// </summary>
    Task<List<ProgrammingLanguageDtos>> GetAllAsync();

    /// <summary>
    /// Добавление нового языка программирования
    /// </summary>
    /// <param name="dto">передаваемые параметры из запроса</param>
    Task<bool?> Create(CreateProgrammingLanguageRequestDto dto);

    /// <summary>
    /// Поиск определенного языка программирования
    /// </summary>
    /// <param name="id">передаваемые параметры из запроса</param>
    Task<ProgrammingLanguageDtos?> Read(Guid id);

    /// <summary>
    /// Изменение определенного языка программирования
    /// </summary>
    /// <param name="programmingLanguage">передаваемые параметры из запроса</param>
    Task<ProgrammingLanguageDtos?> Update(UpdateProgrammingLanguageRequestDto dto);

    /// <summary>
    /// Удаление определенного языка программирования
    /// </summary>
    /// <param name="id">передаваемые параметры из запроса</param>
    Task<bool> Delete(Guid id);
}