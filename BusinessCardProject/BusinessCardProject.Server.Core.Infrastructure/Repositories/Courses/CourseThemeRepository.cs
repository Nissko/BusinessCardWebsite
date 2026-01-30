using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using BusinessCardProject.Server.Core.Domain.Enums.Course;
using ContractualDtos.DTO.Course.CourseTheme.Dtos;
using ContractualDtos.DTO.Course.CourseTheme.Requests;
using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardProject.Server.Core.Infrastructure.Repositories.Courses;

public class CourseThemeRepository : ICourseThemeRepository
{
    private readonly IProjectDbContext _context;

    public CourseThemeRepository(IProjectDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<CourseThemeDtos>> GetAllAsync()
    {
        try
        {
            var data = await _context.CourseTheme.ToListAsync();
            return GetCourseThemeDto(data);
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
                TypeOfCourseEnum.FromId(dto.TypeOfCourseId));

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

    public async Task<CourseThemeDtos?> Read(Guid id)
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

    public async Task<CourseThemeDtos?> Update(UpdateCourseThemeRequestDto dto)
    {
        try
        {
            var courseTheme = await _context.CourseTheme.FindAsync([dto.Id]);
            if (courseTheme == null) return null;

            if (!courseTheme.ProgrammingLanguages.Id.Equals(dto.ProgrammingLanguageId))
            {
                var programLanguage = await _context.ProgrammingLanguageCourse.FindAsync([dto.ProgrammingLanguageId]);
                if (programLanguage == null) return null;
                
                courseTheme.Update(dto.Name, dto.Description, dto.TypeOfCourseId, programLanguage);
            }
            else
            {
                courseTheme.Update(dto.Name, dto.Description, dto.TypeOfCourseId);
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
    private static CourseThemeDtos GetCourseThemeDto(CourseThemeEntity entity)
    {
        return new CourseThemeDtos(
            entity.Id,
            entity.ThemeName,
            entity.ThemeDescription,
            entity.TypeOfCourse.Id,
            new ProgrammingLanguageDtos(entity.ProgrammingLanguages.Id, entity.ProgrammingLanguages.Name,
                entity.ProgrammingLanguages.CountSelectedUser)
        );
    }

    /// <summary>
    /// Формирование DTOs для return
    /// </summary>
    private static List<CourseThemeDtos> GetCourseThemeDto(List<CourseThemeEntity> entities)
    {
        return entities.Select(e => new CourseThemeDtos(
            e.Id,
            e.ThemeName,
            e.ThemeDescription,
            e.TypeOfCourse.Id,
            new ProgrammingLanguageDtos(e.ProgrammingLanguages.Id, e.ProgrammingLanguages.Name,
                e.ProgrammingLanguages.CountSelectedUser)
        )).ToList();
    }
}