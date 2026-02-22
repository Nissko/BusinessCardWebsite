using BusinessCardProject.Server.Core.Application.Application.Extensions;
using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using BusinessCardProject.Server.Core.Domain.Aggregates.Course.CourseTheme;
using BusinessCardProject.Server.Core.Domain.Enums.Course;
using ContractualDtos.DTO.Course.CourseModule.Dtos;
using ContractualDtos.DTO.Course.CourseTheme.Dtos;
using ContractualDtos.DTO.Course.CourseTheme.Requests;
using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;
using ContractualDtos.DTO.Course.VideoCourse.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardProject.Server.Core.Infrastructure.Repositories.Courses
{
    public class CourseThemeRepository : ICourseThemeRepository
    {
        private readonly IProjectDbContext _context;

        public CourseThemeRepository(IProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<DetailedCourseThemeDtos>> GetAllAsync()
        {
            try
            {
                return await _context.CourseTheme
                    .AsNoTracking()
                    .Where(ct => EF.Property<bool>(ct, "_isActive"))
                    .OrderBy(ct => EF.Property<int>(ct, "_displayOrder"))
                    .Include(tr => tr.ThemeRecommendations)
                    .Include(cm => cm.CourseModules)
                    .ThenInclude(vc => vc.VideoCourses)
                    .Include(pl => pl.ProgrammingLanguages)
                    .Select(ct => GetCourseThemeDto(ct))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool?> Create(CreateCourseThemeRequestDto dto)
        {
            try
            {
                var newCourseTheme = new CourseThemeEntity(dto.Name, dto.Description, dto.ProgrammingLanguageId,
                    TypeOfCourseEnum.FromId(dto.TypeOfCourseId), dto.DisplayOrder, dto.IsActive, dto.IsFree);

                var programLanguage = await _context.ProgrammingLanguageCourse.FindAsync([dto.ProgrammingLanguageId]);
                if (programLanguage == null) return null;

                programLanguage.AddTheme(newCourseTheme);
                _context.CourseTheme.Add(newCourseTheme);
                await SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DetailedCourseThemeDtos?> Read(Guid id)
        {
            try
            {
                var courseTheme = await _context.CourseTheme.FindAsync([id]);
                if (courseTheme == null) return null;

                return GetCourseThemeDto(courseTheme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<DetailedCourseThemeDtos?>> FindByProgramLanguage(Guid id)
        {
            try
            {
                return (await _context.CourseTheme
                    .AsNoTracking()
                    .Where(ct => EF.Property<bool>(ct, "_isActive"))
                    .Where(pl => pl.ProgrammingLanguageId == id)
                    .OrderBy(ct => EF.Property<int>(ct, "_displayOrder"))
                    .Include(tr => tr.ThemeRecommendations)
                    .Include(cm => cm.CourseModules)
                    .ThenInclude(vc => vc.VideoCourses)
                    .Include(pl => pl.ProgrammingLanguages)
                    .Select(ct => GetCourseThemeDto(ct))
                    .ToListAsync())!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DetailedCourseThemeDtos?> Update(UpdateCourseThemeRequestDto dto)
        {
            try
            {
                var courseTheme = await _context.CourseTheme.FindAsync([dto.Id]);
                if (courseTheme == null) return null;

                if (!courseTheme.ProgrammingLanguages.Id.Equals(dto.ProgrammingLanguageId))
                {
                    var programLanguage = await _context.ProgrammingLanguageCourse.FindAsync([dto.ProgrammingLanguageId]);
                    if (programLanguage == null) return null;

                    courseTheme.Update(dto.Name, dto.Description, dto.TypeOfCourseId, programLanguage, dto.DisplayOrder,
                        dto.IsActive, dto.IsFree);
                }
                else
                {
                    courseTheme.Update(dto.Name, dto.Description, dto.TypeOfCourseId, dto.DisplayOrder, dto.IsActive,
                        dto.IsFree);
                }

                _context.CourseTheme.Update(courseTheme);
                await SaveChanges();

                return GetCourseThemeDto(courseTheme);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var courseTheme = await _context.CourseTheme.FindAsync([id]);
                if (courseTheme == null) return false;

                _context.CourseTheme.Remove(courseTheme);
                await SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Формирование DTO для return
        /// </summary>
        private static DetailedCourseThemeDtos GetCourseThemeDto(CourseThemeEntity entity)
        {
            return new DetailedCourseThemeDtos(
                entity.Id,
                entity.ThemeName,
                entity.ThemeDescription,
                entity.TypeOfCourse.Id,
                new ProgrammingLanguageDtos(entity.ProgrammingLanguages.Id, entity.ProgrammingLanguages.Name,
                    entity.ProgrammingLanguages.CountSelectedUser),
                entity.ThemeRecommendations.Select(e => new CourseThemeRecommendationDtos(
                    e.Id,
                    e.Title
                )).ToList(),
                entity.CourseModules.Select(e => new CourseModuleDtos(
                    e.Id,
                    e.Name,
                    e.Description,
                    e.VideoCourses.Select(vc => new VideoCourseDtos(
                        vc.Id,
                        vc.Name,
                        vc.Description,
                        vc.Img,
                        vc.DatePublished.ToLocalString(),
                        vc.Price,
                        vc.Discount,
                        vc.Rate,
                        vc.IsFree,
                        vc.CourseAuthorId,
                        vc.CourseModuleId,
                        vc.LinkOnYoutube,
                        vc.LinkOnRutube,
                        vc.LinkOnVkVideo)).ToList()
                )).ToList(),
                entity.DisplayOrder,
                entity.IsActive,
                entity.IsFree
            );
        }

        /// <summary>
        /// Формирование DTOs для return
        /// </summary>
        private static List<DetailedCourseThemeDtos> GetCourseThemeDto(List<CourseThemeEntity> entities)
        {
            return entities.Select(e => new DetailedCourseThemeDtos(
                e.Id,
                e.ThemeName,
                e.ThemeDescription,
                e.TypeOfCourse.Id,
                new ProgrammingLanguageDtos(e.ProgrammingLanguages.Id, e.ProgrammingLanguages.Name,
                    e.ProgrammingLanguages.CountSelectedUser),
                e.ThemeRecommendations.Select(ex => new CourseThemeRecommendationDtos(
                    ex.Id,
                    ex.Title
                )).ToList(),
                e.CourseModules.Select(cm => new CourseModuleDtos(
                    cm.Id,
                    cm.Name,
                    cm.Description,
                    cm.VideoCourses.Select(vc => new VideoCourseDtos(
                        vc.Id,
                        vc.Name,
                        vc.Description,
                        vc.Img,
                        vc.DatePublished.ToLocalString(),
                        vc.Price,
                        vc.Discount,
                        vc.Rate,
                        vc.IsFree,
                        vc.CourseAuthorId,
                        vc.CourseModuleId,
                        vc.LinkOnYoutube,
                        vc.LinkOnRutube,
                        vc.LinkOnVkVideo)).ToList()
                )).ToList(),
                e.DisplayOrder,
                e.IsActive,
                e.IsFree
            )).ToList();
        }
    }
}