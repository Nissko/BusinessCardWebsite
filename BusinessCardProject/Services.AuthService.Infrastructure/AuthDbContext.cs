using Microsoft.EntityFrameworkCore;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Domain.Entities;
using Services.AuthService.Infrastructure.Configurations;

namespace Services.AuthService.Infrastructure
{
    public class AuthDbContext : DbContext, IAuthDbContext
    {
        private readonly string _defaultSchema = "bytecode_auth";

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<RefreshTokenEntity> RefreshToken { get; set; }

        public void Migrate()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_defaultSchema);

            #region user

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        }
    }
}