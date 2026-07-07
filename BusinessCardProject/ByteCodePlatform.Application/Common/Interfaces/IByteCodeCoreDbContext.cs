using ByteCodePlatform.Domain.Entities;
using ByteCodePlatform.Domain.Entities.Course;
using ByteCodePlatform.Domain.Entities.Course.Content;
using ByteCodePlatform.Domain.Entities.Course.Module;
using ByteCodePlatform.Domain.Entities.Course.Theme;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ByteCodePlatform.Application.Common.Interfaces
{
    public interface IByteCodeCoreDbContext
    {
        DatabaseFacade Database { get; }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<AuthorEntity> Author { get; set; }
        public DbSet<ProgrammingLanguageCategoryEntity> ProgrammingLanguageCategory { get; set; }
        public DbSet<CourseThemeEntity> CourseTheme { get; set; }
        public DbSet<CourseThemeFieldPropertyEntity> CourseThemeFieldProperty { get; set; }
        public DbSet<CourseModuleEntity> CourseModule { get; set; }
        public DbSet<CourseModuleFieldPropertyEntity> CourseModuleFieldProperty { get; set; }
        public DbSet<CourseContentEntity> CourseContent { get; set; }
        public DbSet<CourseContentFieldPropertyEntity> CourseContentFieldProperty { get; set; }

        void Migrate();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}