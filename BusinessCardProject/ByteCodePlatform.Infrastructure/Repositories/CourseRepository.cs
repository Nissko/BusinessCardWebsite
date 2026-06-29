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
        private readonly IByteCodeCoreDbContext _dbContext;
        private readonly IMediator _mediator;

        public CourseRepository(IByteCodeCoreDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public Task<CheckGrpcCourseTimingDto> CheckGrpcCourseTiming()
        {
            try
            {
                var dateTimeNow = SystemClock.Instance.GetCurrentInstant();
                return Task.FromResult(new CheckGrpcCourseTimingDto(dateTimeNow, true, Servicename));
            }
            catch (Exception exception)
            {
                return Task.FromException<CheckGrpcCourseTimingDto>(exception);
            }
        }

        #region ProgrammingLanguage
        
        public async Task<List<ProgrammingLanguageDto>> GetProgrammingLanguages()
        {
            var programmingLanguages = await _dbContext.ProgrammingLanguageCategory.ToListAsync();
            return programmingLanguages.GetProgrammingLanguageDtos();
        }

        #endregion

        #region CourseTheme
        
        public async Task<CourseThemeDto> AddCourseTheme(CreateCourseThemeRequest request)
        {
            var programLanguage = await _dbContext.ProgrammingLanguageCategory
                                      .FirstAsync(x => x.Id == request.ProgrammingLanguageId) ??
                                  throw new Exception("Programming language not found");
            var author = await _dbContext.Author.FirstOrDefaultAsync(x => x.Id == request.AuthorId) ??
                         throw new Exception("Author not found");
            var newTheme = new CourseThemeEntity(request.Name, request.Description, request.AvatarUrl, request.Price,
                request.OldPrice, programLanguage.Id, author.Id,
                SystemClock.Instance.GetCurrentInstant());
            newTheme.ThemeFieldProperties.SetDefault(newTheme.Id);

            _dbContext.CourseTheme.Add(newTheme);
            _dbContext.CourseThemeFieldProperty.AddRange(newTheme.ThemeFieldProperties);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return newTheme.GetCourseThemeDto();
        }

        public async Task<List<CourseThemeDto>> GetCourseThemes()
        {
            var themes = await _dbContext.CourseTheme.ToListAsync();
            if (themes.Any(theme => !theme.ThemeFieldProperties.CheckProperties()))
            {
                throw new("Course theme properties are not allowed");
            }

            var orderedThemes = themes.Where(x =>
                    string.Equals(x.ThemeFieldProperties
                        .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true", StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value)
                .ThenBy(x => x.CreatedAt).ToList();

            return orderedThemes.GetCourseThemeDtos();
        }

        public async Task<CourseThemeDto> GetCourseTheme(Guid courseId)
        {
            var theme = await _dbContext.CourseTheme.FirstOrDefaultAsync(x => x.Id == courseId) ??
                        throw new ArgumentNullException(nameof(courseId), "Course theme not found");

            return theme.ThemeFieldProperties.CheckProperties()
                ? theme.GetCourseThemeDto()
                : throw new Exception("Course theme properties are not allowed");
        }

        public async Task<CourseThemeDto> UpdateCourseTheme(UpdateCourseThemeRequest request)
        {
            var theme = await _dbContext.CourseTheme.FirstOrDefaultAsync(x => x.Id == request.Id) ??
                        throw new("Course theme not found");
            theme.Update(request.Name, request.Description, request.AvatarUrl, request.Price, request.OldPrice,
                request.ProgrammingLanguageId);

            if (!theme.ThemeFieldProperties.CheckProperties())
                throw new("Course theme properties are not allowed");

            var isShow = theme.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.IsShow);
            var displayOrder = theme.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder);
            var isFree = theme.ThemeFieldProperties.GetProperty(FieldPropertyTypesEnum.IsFree);

            isShow?.UpdateValue(request.IsShow?.ToString());
            displayOrder?.UpdateValue(request.DisplayOrder?.ToString());
            isFree?.UpdateValue(request.IsFree?.ToString());

            _dbContext.CourseTheme.Update(theme);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return theme.GetCourseThemeDto();
        }

        #endregion

        #region CourseModule

        public async Task<CourseModuleDto> AddCourseModule(CreateCourseModuleRequest request)
        {
            var courseTheme = await _dbContext.CourseTheme.FirstOrDefaultAsync(x => x.Id == request.CourseThemeId) ??
                              throw new Exception("Course theme not found");
            var newModule = new CourseModuleEntity(request.Name, courseTheme.Id,
                SystemClock.Instance.GetCurrentInstant());
            newModule.ModuleFieldProperties.SetDefault(newModule.Id);

            _dbContext.CourseModule.Add(newModule);
            await _dbContext.CourseModuleFieldProperty.AddRangeAsync(newModule.ModuleFieldProperties);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return newModule.GetCourseModuleDto();
        }

        public async Task<List<CourseModuleDto>> GetCourseModules()
        {
            var modules = await _dbContext.CourseModule.ToListAsync();
            if (modules.Any(module => !module.ModuleFieldProperties.CheckProperties()))
            {
                throw new("Course module properties are not allowed");
            }

            var orderedModules = modules.Where(x =>
                    string.Equals(x.ModuleFieldProperties
                        .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true", StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.ModuleFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value)
                .ThenBy(x => x.CreatedAt).ToList();

            return orderedModules.GetCourseModuleDtos();
        }

        public async Task<List<CourseModuleDto>> GetCourseModulesFromCourse(Guid courseId)
        {
            var modules = await _dbContext.CourseModule.Where(x => x.CourseThemeId == courseId).ToListAsync();
            if (modules.Any(module => !module.ModuleFieldProperties.CheckProperties()))
            {
                throw new("Course module properties are not allowed");
            }

            var orderedModules = modules.Where(x =>
                    string.Equals(x.ModuleFieldProperties
                        .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true", StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.ModuleFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value)
                .ThenBy(x => x.CreatedAt).ToList();

            return orderedModules.GetCourseModuleDtos();
        }

        public async Task<CourseModuleDto> UpdateCourseModule(UpdateCourseModuleRequest request)
        {
            var module = await _dbContext.CourseModule.FirstOrDefaultAsync(x => x.Id == request.Id) ??
                         throw new("Course module not found");
            module.Update(request.Name, request.CourseThemeId);

            if (!module.ModuleFieldProperties.CheckProperties())
                throw new("Course module properties are not allowed");

            var isShow = module.ModuleFieldProperties.GetProperty(FieldPropertyTypesEnum.IsShow);
            var displayOrder = module.ModuleFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder);

            isShow?.UpdateValue(request.IsShow?.ToString());
            displayOrder?.UpdateValue(request.DisplayOrder?.ToString());

            _dbContext.CourseModule.Update(module);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return module.GetCourseModuleDto();
        }

        #endregion

        #region CourseContent

        public async Task<CourseContentDto> AddCourseContent(CreateCourseContentRequest request)
        {
            var courseModule = await _dbContext.CourseModule
                                   .FirstOrDefaultAsync(x => x.Id == request.CourseModuleId) ??
                               throw new Exception("Course module not found");
            var newContent = new CourseContentEntity(request.Name, request.LinkOnRutube, request.LinkOnVk,
                request.LinkOnYoutube, request.ImgUrl, SystemClock.Instance.GetCurrentInstant(),
                courseModule.Id);
            newContent.ContentFieldProperties.SetDefault(newContent.Id);

            _dbContext.CourseContent.Add(newContent);
            await _dbContext.CourseContentFieldProperty.AddRangeAsync(newContent.ContentFieldProperties);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return newContent.GetCourseContentDto();
        }

        public async Task<List<CourseContentDto>> GetCourseContents()
        {
            var contents = await _dbContext.CourseContent.ToListAsync();
            if (contents.Any(theme => !theme.ContentFieldProperties.CheckProperties()))
            {
                throw new("Course content properties are not allowed");
            }

            var orderedContents = contents.Where(x =>
                    string.Equals(x.ContentFieldProperties
                        .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true", StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.ContentFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value)
                .ThenBy(x => x.CreatedAt).ToList();

            return orderedContents.GetCourseContentDtos();
        }

        public async Task<List<CourseContentDto>> GetCourseContentsFromModuleId(Guid moduleId)
        {
            var contents = await _dbContext.CourseContent.Where(x=>x.CourseModuleId == moduleId).ToListAsync();
            if (contents.Any(theme => !theme.ContentFieldProperties.CheckProperties()))
            {
                throw new("Course content properties are not allowed");
            }

            var orderedContents = contents.Where(x =>
                    string.Equals(x.ContentFieldProperties
                        .GetProperty(FieldPropertyTypesEnum.IsShow)?.Value, "true", StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => x.ContentFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder)?.Value)
                .ThenBy(x => x.CreatedAt).ToList();

            return orderedContents.GetCourseContentDtos();
        }

        public async Task<CourseContentDto> UpdateCourseContent(UpdateCourseContentRequest request)
        {
            var courseModule = await _dbContext.CourseModule.FirstOrDefaultAsync(x => x.Id == request.CourseModuleId) ??
                               null;
            var content = await _dbContext.CourseContent.FirstOrDefaultAsync(x => x.Id == request.Id) ??
                          throw new("Course content not found");
            content.Update(request.Name, request.LinkOnRutube, request.LinkOnVk, request.LinkOnYoutube, request.ImgUrl,
                courseModule?.Id);

            if (!content.ContentFieldProperties.CheckProperties())
                throw new("Course content properties are not allowed");

            var isShow = content.ContentFieldProperties.GetProperty(FieldPropertyTypesEnum.IsShow);
            var displayOrder = content.ContentFieldProperties.GetProperty(FieldPropertyTypesEnum.DisplayOrder);

            isShow?.UpdateValue(request.IsShow?.ToString());
            displayOrder?.UpdateValue(request.DisplayOrder?.ToString());

            _dbContext.CourseContent.Update(content);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return content.GetCourseContentDto();
        }

        #endregion
    }
}