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

        public AuthDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=192.168.1.130;User Id=nikita;Password=qwertyaib12345678;Port=5432;Database=bytecodeDb;",
                npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
        }

        private static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .Options;
        }
    }
}