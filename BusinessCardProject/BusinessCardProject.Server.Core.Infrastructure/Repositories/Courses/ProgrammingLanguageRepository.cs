using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using ContractualDtos.DTO.ProgrammingLanguageCourse.Dtos;
using ContractualDtos.DTO.ProgrammingLanguageCourse.Requests;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardProject.Server.Core.Infrastructure.Repositories.Courses;

public class ProgrammingLanguageRepository : IProgrammingLanguageRepository
{
    private readonly IProjectDbContext _context;

    public ProgrammingLanguageRepository(IProjectDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<ProgrammingLanguageDtos>> GetAllAsync()
    {
        var data = await _context.ProgrammingLanguageCourse.ToListAsync();

        return data.Select(e => new ProgrammingLanguageDtos(
            e.Id,
            e.Name,
            e.CountSelectedUser
        )).ToList();
    }

    public async Task<ProgrammingLanguageDtos> Create(CreateProgrammingLanguageRequestDto dto)
    {
        var newProgrammingLanguage = new ProgrammingLanguageCourseEntity(dto.Name, 0);
        _context.ProgrammingLanguageCourse.Add(newProgrammingLanguage);
        await SaveChanges();

        return new ProgrammingLanguageDtos(newProgrammingLanguage.Id, newProgrammingLanguage.Name,
            newProgrammingLanguage.CountSelectedUser);
    }

    public async Task<ProgrammingLanguageDtos?> Read(Guid id)
    {
        var programmingLanguage = await _context.ProgrammingLanguageCourse.FindAsync([id]);
        if (programmingLanguage == null)
        {
            return null;
        }

        return new ProgrammingLanguageDtos(programmingLanguage.Id, programmingLanguage.Name,
            programmingLanguage.CountSelectedUser);
    }

    public async Task<ProgrammingLanguageDtos?> Update(UpdateProgrammingLanguageRequestDto dto)
    {
        var programmingLanguage = await _context.ProgrammingLanguageCourse.FindAsync([dto.Id]);
        if (programmingLanguage == null)
        {
            return null;
        }

        programmingLanguage.Update(dto.Name);
        _context.ProgrammingLanguageCourse.Update(programmingLanguage);
        await SaveChanges();

        return new ProgrammingLanguageDtos(programmingLanguage.Id, programmingLanguage.Name,
            programmingLanguage.CountSelectedUser);
    }

    public async Task<bool> Delete(Guid id)
    {
        var programmingLanguage = await _context.ProgrammingLanguageCourse.FindAsync([id]);

        if (programmingLanguage == null)
        {
            return false;
        }
        
        _context.ProgrammingLanguageCourse.Remove(programmingLanguage);
        await SaveChanges();
        
        return true;
    }

    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}