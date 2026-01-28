using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using BusinessCardProject.Server.Core.Domain.Enums.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.Course;

public class CourseThemeConfiguration : IEntityTypeConfiguration<CourseThemeEntity>
{
    public void Configure(EntityTypeBuilder<CourseThemeEntity> builder)
    {
        builder.ToTable("Themes");
        builder.HasKey(x => x.Id);

        builder.Property<string>("_themeName")
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Название");

        builder.Property<string>("_themeDescription")
            .HasColumnName("Description")
            .HasMaxLength(1000)
            .HasComment("Описание");

        builder.Property<TypeOfCourseEnum>("_typeOfCourseId")
            .HasColumnName("TypeOfCourse")
            .HasConversion(
                v => v.Id,
                v => TypeOfCourseEnum.List().First(t => t.Id == v));

        builder.HasOne(x => x.ProgrammingLanguages)
            .WithMany(x => x.CourseThemes)
            .HasForeignKey("ProgrammingLanguageId")
            .OnDelete(DeleteBehavior.Cascade);

        #region Индексы

        builder.HasIndex("ProgrammingLanguageId");

        #endregion
    }
}