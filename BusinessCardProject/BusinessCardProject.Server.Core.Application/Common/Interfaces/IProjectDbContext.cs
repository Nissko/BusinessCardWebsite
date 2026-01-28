using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using BusinessCardProject.Server.Core.Domain.Aggregates.User;
using BusinessCardProject.Server.Core.Domain.Enums.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces;

public interface IProjectDbContext
{
    DatabaseFacade Database { get; }

    public DbSet<ProgrammingLanguageCourseEntity> ProgrammingLanguageCourse { get; set; }
    public DbSet<CourseAuthorEntity> CourseAuthor { get; set; }
    public DbSet<CourseThemeEntity> CourseTheme { get; set; }
    public DbSet<TypeOfCourseEnum> TypeOfCourse { get; set; }
    public DbSet<CourseModuleEntity> CourseModule { get; set; }
    public DbSet<VideoCourseEntity> VideoCourse { get; set; }

    public DbSet<UserRoleEntity> UserRole { get; set; }
    public DbSet<UserProfileEntity> UserProfile { get; set; }

    void Migrate();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}