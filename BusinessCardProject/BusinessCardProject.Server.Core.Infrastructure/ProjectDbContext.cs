using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using BusinessCardProject.Server.Core.Domain.Commons.Interface;
using BusinessCardProject.Server.Core.Domain.Enums.Course;
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

        public DbSet<ProgrammingLanguageCourseEntity> ProgrammingLanguageCourse { get; set; }
        public DbSet<CourseAuthorEntity> CourseAuthor { get; set; }
        public DbSet<CourseThemeEntity> CourseTheme { get; set; }
        public DbSet<TypeOfCourseEnum> TypeOfCourse { get; set; }
        public DbSet<CourseModuleEntity> CourseModule { get; set; }
        public DbSet<VideoCourseEntity> VideoCourse { get; set; }


        public void Migrate()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_defaultSchema);

            modelBuilder.ApplyConfiguration(new TypeOfCourseConfiguration());
            
            modelBuilder.ApplyConfiguration(new ProgrammingLanguageCourseConfiguration());
            modelBuilder.ApplyConfiguration(new CourseAuthorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CourseThemeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseModuleConfiguration());
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