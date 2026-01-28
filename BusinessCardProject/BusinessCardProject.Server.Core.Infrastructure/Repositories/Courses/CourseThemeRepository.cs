using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using ContractualDtos.DTO.CourseTheme.Dtos;
using ContractualDtos.DTO.CourseTheme.Requests;

namespace BusinessCardProject.Server.Core.Infrastructure.Repositories.Courses;

public class CourseThemeRepository : ICourseThemeRepository
{
    private readonly IProjectDbContext _context;

    public CourseThemeRepository(IProjectDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<List<CourseThemeDtos>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<CourseThemeDtos> Create(CreateCourseThemeRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<CourseThemeDtos?> Read(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<CourseThemeDtos?> Update(UpdateCourseThemeRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    private async Task SaveChanges()
    {
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}