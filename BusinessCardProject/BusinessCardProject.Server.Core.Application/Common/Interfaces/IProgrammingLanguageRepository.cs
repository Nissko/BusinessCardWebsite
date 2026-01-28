using ContractualDtos.DTO.ProgrammingLanguageCourse.Dtos;
using ContractualDtos.DTO.ProgrammingLanguageCourse.Requests;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces;

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
    Task<ProgrammingLanguageDtos> Create(CreateProgrammingLanguageRequestDto dto);

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