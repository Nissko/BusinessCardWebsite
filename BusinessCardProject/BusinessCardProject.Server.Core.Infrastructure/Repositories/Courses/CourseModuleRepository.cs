using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using ContractualDtos.DTO.Course.CourseModule.Dtos;
using ContractualDtos.DTO.Course.CourseModule.Requests;
using ContractualDtos.DTO.Course.CourseTheme.Dtos;
using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardProject.Server.Core.Infrastructure.Repositories.Courses
{
    public class CourseModuleRepository : ICourseModuleRepository
    {
        private readonly IProjectDbContext _context;

        public CourseModuleRepository(IProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<DetailedCourseModuleDtos>> GetAllAsync()
        {
            try
            {
                return await _context.CourseModule
                    .AsNoTracking()
                    .Select(cm => GetCourseModuleDto(cm))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool?> Create(CreateCourseModuleRequestDto dto)
        {
            try
            {
                var newCourseModule = new CourseModuleEntity(dto.Name, dto.Description, dto.CourseThemeId, dto.IsShow,
                    dto.DisplayOrder);

                var courseTheme = await _context.CourseTheme.FindAsync(dto.CourseThemeId);
                if (courseTheme == null) return null;

                courseTheme.AddModule(newCourseModule);
                _context.CourseModule.Add(newCourseModule);
                await SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<DetailedCourseModuleDtos?> Read(Guid id)
        {
            try
            {
                return await _context.CourseModule
                    .AsNoTracking()
                    .Select(cm => GetCourseModuleDto(cm))
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> Update(UpdateCourseModuleRequestDto dto)
        {
            try
            {
                var courseModule = await _context.CourseModule.FindAsync(dto.Id);
                if (courseModule == null) return false;

                courseModule.Update(dto.Param, dto.Value);
                _context.CourseModule.Update(courseModule);
                await SaveChanges();

                return true;
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
                var courseModule = await _context.CourseModule.FindAsync([id]);
                if (courseModule == null) return false;

                _context.CourseModule.Remove(courseModule);
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
        private static DetailedCourseModuleDtos GetCourseModuleDto(CourseModuleEntity entity)
        {
            return new DetailedCourseModuleDtos(
                entity.Id,
                entity.Name,
                entity.Description,
                entity.DisplayOrder,
                entity.IsShow,
                new CourseThemeDtos(
                    entity.CourseTheme.Id,
                    entity.CourseTheme.Name,
                    entity.CourseTheme.Description,
                    entity.CourseTheme.TypeOfCourse.Id,
                    new ProgrammingLanguageDtos(
                        entity.CourseTheme.ProgrammingLanguages.Id,
                        entity.CourseTheme.ProgrammingLanguages.Name,
                        entity.CourseTheme.ProgrammingLanguages.CountSelectedUser
                    ),
                    entity.CourseTheme.ThemeRecommendations.Select(e => new CourseThemeRecommendationDtos(
                        e.Id,
                        e.Title
                    )).ToList(),
                    entity.CourseTheme.DisplayOrder,
                    entity.CourseTheme.IsActive,
                    entity.CourseTheme.IsFree
                )
            );
        }

        /// <summary>
        /// Формирование DTOs для return
        /// </summary>
        private static List<DetailedCourseModuleDtos> GetCourseModuleDto(List<CourseModuleEntity> entities)
        {
            return entities.Select(e => new DetailedCourseModuleDtos(
                e.Id,
                e.Name,
                e.Description,
                e.DisplayOrder,
                e.IsShow,
                new CourseThemeDtos(
                    e.CourseTheme.Id,
                    e.CourseTheme.Name,
                    e.CourseTheme.Description,
                    e.CourseTheme.TypeOfCourse.Id,
                    new ProgrammingLanguageDtos(
                        e.CourseTheme.ProgrammingLanguages.Id,
                        e.CourseTheme.ProgrammingLanguages.Name,
                        e.CourseTheme.ProgrammingLanguages.CountSelectedUser
                    ),
                    e.CourseTheme.ThemeRecommendations.Select(ex => new CourseThemeRecommendationDtos(
                        ex.Id,
                        ex.Title
                    )).ToList(),
                    e.CourseTheme.DisplayOrder,
                    e.CourseTheme.IsActive,
                    e.CourseTheme.IsFree
                )
            )).ToList();
        }
    }
}