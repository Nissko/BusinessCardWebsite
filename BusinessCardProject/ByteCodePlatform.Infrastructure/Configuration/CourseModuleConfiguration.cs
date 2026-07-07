using ByteCodePlatform.Domain.Entities.Course.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteCodePlatform.Infrastructure.Configuration
{
    public class CourseModuleConfiguration : IEntityTypeConfiguration<CourseModuleEntity>
    {
        public void Configure(EntityTypeBuilder<CourseModuleEntity> builder)
        {
            builder.ToTable("CourseModules");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasComment("Название модуля");
        
            builder.Property(x => x.CreatedAt)
                .HasComment("Дата добавления");
        
            builder.Property(x => x.UpdatedAt)
                .IsRequired(false)
                .HasComment("Дата удаления");
        
            builder.HasOne(x => x.CourseTheme)
                .WithMany(x => x.CourseModules)
                .HasForeignKey(x => x.CourseThemeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}