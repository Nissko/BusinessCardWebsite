using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using BusinessCardProject.Server.Core.Domain.Commons.Interface;
using BusinessCardProject.Server.Core.Infrastructure.Configuration.Course;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardProject.Server.Core.Infrastructure
{
    public sealed class ProjectDbContext : DbContext, IProjectDbContext
    {
        private readonly string _defaultSchema = "dev_prod";

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<VideoCourseEntity> VideoCourse { get; set; }

        public void Migrate()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_defaultSchema);

            modelBuilder.ApplyConfiguration(new VideoCourseConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectDbContext).Assembly);
        }

        public ProjectDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Адрес тестового сервера
            optionsBuilder
                .UseNpgsql(
                    "Server=109.205.58.47;User Id=persProjectUser;Password=7FEpX_wl6g;Port=5432;Database=testDb;")
                .UseLazyLoadingProxies();
        }

        private static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .Options;
        }
    }
}