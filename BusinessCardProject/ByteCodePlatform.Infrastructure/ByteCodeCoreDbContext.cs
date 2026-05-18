using ByteCodePlatform.Application.Common.Interfaces;
using ByteCodePlatform.Domain.Entities;
using ByteCodePlatform.Domain.Entities.Course;
using ByteCodePlatform.Domain.Entities.Course.Content;
using ByteCodePlatform.Domain.Entities.Course.Module;
using ByteCodePlatform.Domain.Entities.Course.Theme;
using ByteCodePlatform.Infrastructure.Configuration;
using ByteCodePlatform.Infrastructure.Configuration.FieldProperties;
using Microsoft.EntityFrameworkCore;

namespace ByteCodePlatform.Infrastructure
{
    public class ByteCodeCoreDbContext : DbContext, IByteCodeCoreDbContext
    {
        private readonly string _defaultSchema = "bytecode_core";

        public ByteCodeCoreDbContext(DbContextOptions<ByteCodeCoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<AuthorEntity> Author { get; set; }
        public DbSet<ProgrammingLanguageCategoryEntity> ProgrammingLanguageCategory { get; set; }
        public DbSet<CourseThemeEntity> CourseTheme { get; set; }
        public DbSet<CourseThemeFieldPropertyEntity> CourseThemeFieldProperty { get; set; }
        public DbSet<CourseModuleEntity> CourseModule { get; set; }
        public DbSet<CourseModuleFieldPropertyEntity> CourseModuleFieldProperty { get; set; }
        public DbSet<CourseContentEntity> CourseContent { get; set; }
        public DbSet<CourseContentFieldPropertyEntity> CourseContentFieldProperty { get; set; }

        public void Migrate()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_defaultSchema);

            #region user

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());

            #endregion

            #region couser

            modelBuilder.ApplyConfiguration(new ProgrammingLanguageCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CourseThemeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseModuleConfiguration());
            modelBuilder.ApplyConfiguration(new CourseContentConfiguration());

            modelBuilder.ApplyConfiguration(new FieldPropertyTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseThemeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseModuleConfiguration());
            modelBuilder.ApplyConfiguration(new CourseContentConfiguration());

            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ByteCodeCoreDbContext).Assembly);
        }

        public ByteCodeCoreDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=193.42.115.251;User Id=nikita;Password=qwertyaib12345678;Port=5432;Database=bytecodeDb;",
                npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
            //optionsBuilder.UseNpgsql("Server=localhost;User Id=postgres;Password=0000;Port=5432;Database=bytecodeDb;",
            //npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
        }

        private static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .Options;
        }
    }
}