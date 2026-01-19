using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BusinessCardProject.Server.Core.Domain.Commons.Interface;

public interface IProjectDbContext
{
    DatabaseFacade  Database { get; }

    void Migrate();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}