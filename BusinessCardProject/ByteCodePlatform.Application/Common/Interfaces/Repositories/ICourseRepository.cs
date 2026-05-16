using Dtos.DTO.Course.Content;
using Dtos.DTO.Course.Module;
using Dtos.DTO.Course.ProgramLanguage;
using Dtos.DTO.Course.Theme;
using Requests.Course.Content;
using Requests.Course.Module;
using Requests.Course.Theme;

namespace ByteCodePlatform.Application.Common.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        #region ProgrammingLanguage

        /// <summary>
        /// Вывод ЯП
        /// </summary>
        Task<List<ProgrammingLanguageDto>> GetProgrammingLanguages();

        #endregion

        #region CourseTheme

        /// <summary>
        /// Добавление темы курса
        /// </summary>
        Task<CourseThemeDto> AddCourseTheme(CreateCourseThemeRequest request);

        /// <summary>
        /// Вывод тем
        /// </summary>
        Task<List<CourseThemeDto>> GetCourseThemes();

        /// <summary>
        /// Изменение информации темы
        /// </summary>
        Task<CourseThemeDto> UpdateCourseTheme(UpdateCourseThemeRequest request);

        #endregion

        #region CourseModule

        /// <summary>
        /// Добавление модуля курса
        /// </summary>
        Task<CourseModuleDto> AddCourseModule(CreateCourseModuleRequest request);

        /// <summary>
        /// Вывод модулей курса
        /// </summary>
        Task<List<CourseModuleDto>> GetCourseModules();

        /// <summary>
        /// Изменение модуля курса
        /// </summary>
        Task<CourseModuleDto> UpdateCourseModule(UpdateCourseModuleRequest request);

        #endregion

        #region CourseContent

        /// <summary>
        /// Добавление контента курса
        /// </summary>
        Task<CourseContentDto> AddCourseContent(CreateCourseContentRequest request);

        /// <summary>
        /// Вывод контента курсов
        /// </summary>
        Task<List<CourseContentDto>> GetCourseContents();

        /// <summary>
        /// Изменение контента курса
        /// </summary>
        Task<CourseContentDto> UpdateCourseContent(UpdateCourseContentRequest request);

        #endregion
    }
}