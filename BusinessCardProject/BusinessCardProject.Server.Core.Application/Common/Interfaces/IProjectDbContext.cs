using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using BusinessCardProject.Server.Core.Domain.Aggregates.Course.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces;

public interface IProjectDbContext
{
    DatabaseFacade Database { get; }

    #region Course

    public DbSet<VideoCourseEntity> VideoCourse { get; set; }
    
    #endregion

    void Migrate();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}