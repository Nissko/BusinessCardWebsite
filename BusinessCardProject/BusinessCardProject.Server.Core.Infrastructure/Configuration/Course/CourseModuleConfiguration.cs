using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.Course
{
    public class CourseModuleConfiguration : IEntityTypeConfiguration<CourseModuleEntity>
    {
        public void Configure(EntityTypeBuilder<CourseModuleEntity> builder)
        {
            builder.ToTable("Modules");
            builder.HasKey(x => x.Id);

            builder.Property("_name")
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("Название");

            builder.Property("_description")
                .HasColumnName("Description")
                .HasMaxLength(1000)
                .HasComment("Описание");

            builder.Property("_displayOrder")
                .HasColumnName("DisplayOrder")
                .HasDefaultValue(1)
                .HasComment("Порядок отображения");

            builder.Property("_isShow")
                .HasColumnName("IsShow")
                .HasDefaultValue(false)
                .HasComment("Вывод");

            builder.HasOne(x => x.CourseTheme)
                .WithMany(x => x.CourseModules)
                .HasForeignKey(x=>x.CourseThemeId)
                .OnDelete(DeleteBehavior.SetNull);

            #region Индексы

            builder.HasIndex(x=>x.CourseThemeId);

            #endregion
        }
    }
}