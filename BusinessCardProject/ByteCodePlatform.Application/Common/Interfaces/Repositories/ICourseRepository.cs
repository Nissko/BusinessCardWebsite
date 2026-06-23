using Dtos.DTO.Course;
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
        
        /// <summary>
        /// Проверка состояния gRPC
        /// </summary>
        Task<CheckGrpcCourseTimingDto> CheckGrpcCourseTiming();
        
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
        /// Вывод темы по Id
        /// </summary>
        Task<CourseThemeDto> GetCourseTheme(Guid courseId);

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
        /// Вывод всех модулей курсов
        /// </summary>
        Task<List<CourseModuleDto>> GetCourseModules();
        
        /// <summary>
        /// Вывод всех модулей курса
        /// </summary>
        Task<List<CourseModuleDto>> GetCourseModulesFromCourse(Guid courseId);

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
        /// Вывод контента модуля
        /// </summary>
        Task<List<CourseContentDto>> GetCourseContentsFromModuleId(Guid moduleId);

        /// <summary>
        /// Изменение контента курса
        /// </summary>
        Task<CourseContentDto> UpdateCourseContent(UpdateCourseContentRequest request);

        #endregion
    }
}