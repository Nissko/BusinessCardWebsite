using ByteCodePlatform.Application.Common.Interfaces;
using ByteCodePlatform.Application.Common.Interfaces.Repositories;
using ByteCodePlatform.Domain.Entities.Course.Content;
using ByteCodePlatform.Domain.Entities.Course.Module;
using ByteCodePlatform.Domain.Entities.Course.Theme;
using ByteCodePlatform.Domain.Enums;
using ByteCodePlatform.Domain.Extensions;
using ByteCodePlatform.Domain.Extensions.Course;
using Dtos.DTO.Course;
using Dtos.DTO.Course.Content;
using Dtos.DTO.Course.Module;
using Dtos.DTO.Course.ProgramLanguage;
using Dtos.DTO.Course.Theme;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Requests.Course.Content;
using Requests.Course.Module;
using Requests.Course.Theme;

namespace ByteCodePlatform.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private const string Servicename = "CourseGrpcService";
        private readonly IByteCodeCoreDbContext _context;
        private readonly IMediator _mediator;

        public CourseRepository(IByteCodeCoreDbContext dbContext, IMediator mediator)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<CheckGrpcCourseTimingDto> CheckGrpcCourseTiming()
        {
            try
            {
                var dateTimeNow = SystemClock.Instance.GetCurrentInstant();
                return await Task.FromResult(new CheckGrpcCourseTimingDto(dateTimeNow, true, Servicename));
            }
            catch (Exception exception)
            {
                return await Task.FromException<CheckGrpcCourseTimingDto>(exception);
            }
        }

        #region ProgrammingLanguage

        public async Task<List<ProgrammingLanguageDto>> GetProgrammingLanguages()
        {
            var programmingLanguages = await _context.ProgrammingLanguageCategory.ToListAsync();
            return programmingLanguages.GetProgrammingLanguageDtos();
        }

        #endregion

        #region CourseTheme

        public async Task<CourseThemeDto> AddCourseTheme(CreateCourseThemeRequest request)
        {
            var programLanguage = await _context.ProgrammingLanguageCategory
                                      .FirstAsync(x => x.Id == request.ProgrammingLanguageId) ??
                                  throw new Exception("Язык программирования не найден");
            var author = await _context.Author.FirstOrDefaultAsync(x => x.Id == request.AuthorId) ??
                         throw new Exception("Автор не найден");
            var newTheme = new CourseThemeEntity(request.Name, request.Description, request.AvatarUrl, request.Price,
                request.OldPrice, programLanguage.Id, author.Id,
                SystemClock.Instance.GetCurrentInstant());
            newTheme.ThemeFieldProperties.SetDefault(newTheme.Id);

            _context.CourseTheme.Add(newTheme);
            _context.CourseThemeFieldProperty.AddRange(newTheme.ThemeFieldProperties);
            await _context.SaveChangesAsync(CancellationToken.None);

            return newTheme.GetCourseThemeDto();
        }

        public async Task<List<CourseThemeDto>> GetCourseThemes(bool ignoreFilters)
        {
            var themes = await _context.CourseTheme.ToListAsync();
            if (themes.Any(theme => !theme.ThemeFieldProperties.CheckProperties()))
            {
                throw new("Свойства темы курса не найдены");
            }

            List<CourseThemeEntity> orderedThemes;
            if (!ignoreFilters)
            {
                orderedThemes = themes.Where(x =>
                        string.Equals(x.ThemeFieldProperties
                                .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true",
                            StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(x =>
                        int.Parse(x.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value ?? ""))
                    .ThenBy(x => x.CreatedAt).ToList();
            }
            else
            {
                orderedThemes = themes
                    .OrderBy(x =>
                        int.Parse(x.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value ?? ""))
                    .ThenBy(x => x.CreatedAt).ToList();
            }

            return orderedThemes.GetCourseThemeDtos();
        }

        public async Task<CourseThemeDto> GetCourseTheme(Guid courseId)
        {
            var theme = await _context.CourseTheme.FirstOrDefaultAsync(x => x.Id == courseId) ??
                        throw new ArgumentNullException(nameof(courseId), "Course theme not found");

            return theme.ThemeFieldProperties.CheckProperties()
                ? theme.GetCourseThemeDto()
                : throw new Exception("Свойства темы курса не найдены");
        }

        public async Task<CourseThemeDto> UpdateCourseTheme(UpdateCourseThemeRequest request)
        {
            var theme = await _context.CourseTheme.FirstOrDefaultAsync(x => x.Id == request.Id) ??
                        throw new("Тема курса не найдена");
            theme.Update(request.Name, request.Description, request.AvatarUrl, request.Price, request.OldPrice,
                request.ProgrammingLanguageId);

            if (!theme.ThemeFieldProperties.CheckProperties())
                throw new("Свойства темы курса не найдены");

            var isShow = theme.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.IsShow);
            var displayOrder = theme.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder);
            var isFree = theme.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.IsFree);
            var isDiscount = theme.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.IsDiscount);

            if (request.IsShow != null)
            {
                isShow?.UpdateValue(request.IsShow?.ToString());
            }

            if (request.IsFree != null)
            {
                isFree?.UpdateValue(request.IsFree?.ToString());
            }

            if (request.DisplayOrder != null)
            {
                displayOrder?.UpdateValue(request.DisplayOrder?.ToString());
            }

            if (request.IsDiscount != null)
            {
                isDiscount?.UpdateValue(request.IsDiscount?.ToString());
            }

            _context.CourseTheme.Update(theme);
            await _context.SaveChangesAsync(CancellationToken.None);

            return theme.GetCourseThemeDto();
        }

        public async Task<CourseThemePropertiesDto> GetCourseThemeProperties(Guid courseThemeId)
        {
            var courseThemeProperties = await _context.CourseThemeFieldProperty
                                            .AsNoTracking()
                                            .Where(x => x.CourseThemeId == courseThemeId)
                                            .ToListAsync() ??
                                        throw new Exception("Свойства темы курса не найдены");
            return courseThemeProperties.GetCourseThemePropertiesDto();
        }

        #endregion

        #region CourseModule

        public async Task<CourseModuleDto> AddCourseModule(CreateCourseModuleRequest request)
        {
            var courseTheme = await _context.CourseTheme.FirstOrDefaultAsync(x => x.Id == request.CourseThemeId) ??
                              throw new Exception("Тема курса не найдена");
            var newModule = new CourseModuleEntity(request.Name, courseTheme.Id,
                SystemClock.Instance.GetCurrentInstant());
            newModule.ModuleFieldProperties.SetDefault(newModule.Id);

            _context.CourseModule.Add(newModule);
            await _context.CourseModuleFieldProperty.AddRangeAsync(newModule.ModuleFieldProperties);
            await _context.SaveChangesAsync(CancellationToken.None);

            return newModule.GetCourseModuleDto();
        }

        public async Task<List<CourseModuleDto>> GetCourseModules()
        {
            var modules = await _context.CourseModule.ToListAsync();
            if (modules.Any(module => !module.ModuleFieldProperties.CheckProperties()))
            {
                throw new("Свойства модуля курса не найдены");
            }

            var orderedModules = modules.Where(x =>
                    string.Equals(x.ModuleFieldProperties
                            .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true",
                        StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x =>
                    int.Parse(x.ModuleFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value ?? ""))
                .ThenBy(x => x.CreatedAt).ToList();

            return orderedModules.GetCourseModuleDtos();
        }

        public async Task<List<CourseModuleDto>> GetCourseModulesFromCourse(Guid courseId, bool ignoreFilters)
        {
            var modules = await _context.CourseModule.Where(x => x.CourseThemeId == courseId).ToListAsync();
            if (modules.Any(module => !module.ModuleFieldProperties.CheckProperties()))
            {
                throw new("Свойства модуля курса не найдены");
            }

            List<CourseModuleEntity> orderedModules;
            if (!ignoreFilters)
            {
                orderedModules = modules.Where(x =>
                        string.Equals(x.ModuleFieldProperties
                                .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true",
                            StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(x =>
                        int.Parse(x.ModuleFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value ??
                                  ""))
                    .ThenBy(x => x.CreatedAt).ToList();
            }
            else
            {
                orderedModules = modules
                    .OrderBy(x =>
                        int.Parse(x.ModuleFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value ??
                                  ""))
                    .ThenBy(x => x.CreatedAt).ToList();
            }

            return orderedModules.GetCourseModuleDtos();
        }

        public async Task<CourseModuleDto> UpdateCourseModule(UpdateCourseModuleRequest request)
        {
            var module = await _context.CourseModule.FirstOrDefaultAsync(x => x.Id == request.Id) ??
                         throw new("Модуль курса не найден");
            module.Update(request.Name, request.CourseThemeId);

            if (!module.ModuleFieldProperties.CheckProperties())
                throw new("Использование свойств содержимого курса не найдены");

            var isShow = module.ModuleFieldProperties.GetProperty(FieldPropertyTypesEnum.IsShow);
            var displayOrder = module.ModuleFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder);

            isShow?.UpdateValue(request.IsShow?.ToString());
            displayOrder?.UpdateValue(request.DisplayOrder?.ToString());

            _context.CourseModule.Update(module);
            await _context.SaveChangesAsync(CancellationToken.None);

            return module.GetCourseModuleDto();
        }

        public async Task<CourseModulePropertiesDto> GetCourseModuleProperties(Guid courseModuleId)
        {
            var courseModuleProperties = await _context.CourseModuleFieldProperty
                                             .AsNoTracking()
                                             .Where(x => x.CourseModuleId == courseModuleId)
                                             .ToListAsync() ??
                                         throw new Exception("Свойства модуля курса не найдены");
            return courseModuleProperties.GetCourseModulePropertiesDto();
        }

        #endregion

        #region CourseContent

        public async Task<CourseContentDto> AddCourseContent(CreateCourseContentRequest request)
        {
            var courseModule = await _context.CourseModule
                                   .FirstOrDefaultAsync(x => x.Id == request.CourseModuleId) ??
                               throw new Exception("Модуль курса не найден");
            var newContent = new CourseContentEntity(request.Name, request.LinkOnRutube, request.LinkOnVk,
                request.LinkOnYoutube, request.ImgUrl, SystemClock.Instance.GetCurrentInstant(),
                courseModule.Id);
            newContent.ContentFieldProperties.SetDefault(newContent.Id);

            _context.CourseContent.Add(newContent);
            await _context.CourseContentFieldProperty.AddRangeAsync(newContent.ContentFieldProperties);
            await _context.SaveChangesAsync(CancellationToken.None);

            return newContent.GetCourseContentDto();
        }

        public async Task<List<CourseContentDto>> GetCourseContents()
        {
            var contents = await _context.CourseContent.ToListAsync();
            if (contents.Any(theme => !theme.ContentFieldProperties.CheckProperties()))
            {
                throw new("Использование свойств содержимого курса не найдены");
            }

            var orderedContents = contents.Where(x =>
                    string.Equals(x.ContentFieldProperties
                            .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true",
                        StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x =>
                    int.Parse(x.ContentFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value ?? ""))
                .ThenBy(x => x.CreatedAt).ToList();

            return orderedContents.GetCourseContentDtos();
        }

        public async Task<List<CourseContentDto>> GetCourseContentsFromModuleId(Guid moduleId, bool ignoreFilters)
        {
            var contents = await _context.CourseContent
                .Where(x => x.CourseModuleId == moduleId)
                .ToListAsync();
            if (contents.Any(theme => !theme.ContentFieldProperties.CheckProperties()))
            {
                throw new("Использование свойств содержимого курса не найдены");
            }

            List<CourseContentEntity> orderedContents;
            if (!ignoreFilters)
            {
                orderedContents = contents.Where(x =>
                        string.Equals(x.ContentFieldProperties
                                .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true",
                            StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(x =>
                        int.Parse(
                            x.ContentFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value ?? ""))
                    .ThenBy(x => x.CreatedAt).ToList();
            }
            else
            {
                orderedContents = contents
                    .OrderBy(x =>
                        int.Parse(
                            x.ContentFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value ?? ""))
                    .ThenBy(x => x.CreatedAt).ToList();
            }

            return orderedContents.GetCourseContentDtos();
        }

        public async Task<CourseContentDto> UpdateCourseContent(UpdateCourseContentRequest request)
        {
            var courseModule = await _context.CourseModule.FirstOrDefaultAsync(x => x.Id == request.CourseModuleId) ??
                               null;
            var content = await _context.CourseContent.FirstOrDefaultAsync(x => x.Id == request.Id) ??
                          throw new("Содержание курса не найдено");
            content.Update(request.Name, request.LinkOnRutube, request.LinkOnVk, request.LinkOnYoutube, request.ImgUrl,
                courseModule?.Id);

            if (!content.ContentFieldProperties.CheckProperties())
                throw new("Использование свойств содержимого курса не найдены");

            var isShow = content.ContentFieldProperties.GetProperty(FieldPropertyTypesEnum.IsShow);
            var displayOrder = content.ContentFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder);

            isShow?.UpdateValue(request.IsShow?.ToString());
            displayOrder?.UpdateValue(request.DisplayOrder?.ToString());

            _context.CourseContent.Update(content);
            await _context.SaveChangesAsync(CancellationToken.None);

            return content.GetCourseContentDto();
        }

        public async Task<CourseContentPropertiesDto> GetCourseContentProperties(Guid courseContentId)
        {
            var courseContentProperties = await _context.CourseContentFieldProperty
                                              .AsNoTracking()
                                              .Where(x => x.CourseContentId == courseContentId)
                                              .ToListAsync() ??
                                          throw new Exception("Свойства содержимого курса не найдены");
            return courseContentProperties.GetCourseContentPropertiesDto();
        }

        #endregion
    }
}