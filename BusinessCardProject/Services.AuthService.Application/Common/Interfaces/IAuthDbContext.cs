using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IAuthDbContext
    {
        DatabaseFacade Database { get; }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<UserRolesEntity> UserRole { get; set; }
        public DbSet<RefreshTokenEntity> RefreshToken { get; set; }

        void Migrate();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}