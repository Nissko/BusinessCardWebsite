using ByteCodePlatform.Domain.Entities.Course.Theme;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteCodePlatform.Infrastructure.Configuration
{
    public class CourseThemeConfiguration : IEntityTypeConfiguration<CourseThemeEntity>
    {
        public void Configure(EntityTypeBuilder<CourseThemeEntity> builder)
        {
            builder.ToTable("CourseThemes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasComment("Название");
        
            builder.Property(x => x.Description)
                .HasComment("Описание");
        
            builder.Property(x => x.AvatarUrl)
                .HasComment("Ссылка на обложку");
        
            builder.Property(x => x.Price)
                .HasComment("Текущая цена");
        
            builder.Property(x => x.OldPrice)
                .IsRequired(false)
                .HasComment("Старая цена");
        
            builder.Property(x => x.CreatedAt)
                .HasComment("Дата добавления");
        
            builder.Property(x => x.UpdatedAt)
                .IsRequired(false)
                .HasComment("Дата изменения");

            builder.HasOne(x => x.ProgrammingLanguageCategory)
                .WithMany(x => x.CoursesThemes)
                .HasForeignKey(x => x.ProgrammingLanguageCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        
            builder.HasOne(x => x.Author)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}