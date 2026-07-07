using ByteCodePlatform.Domain.Entities.Course.Theme;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteCodePlatform.Infrastructure.Configuration.FieldProperties
{
    public class CourseThemeFieldPropertyConfiguration : IEntityTypeConfiguration<CourseThemeFieldPropertyEntity>
    {
        public void Configure(EntityTypeBuilder<CourseThemeFieldPropertyEntity> builder)
        {
            builder.ToTable("CourseThemeFieldProperties");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                .IsRequired()
                .HasComment("Значение");
        
            builder.HasOne(x => x.CourseTheme)
                .WithMany(x => x.ThemeFieldProperties)
                .HasForeignKey(x => x.CourseThemeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(x=>x.FieldPropertyType)
                .WithMany(x => x.CourseTheme)
                .HasForeignKey(x => x.FieldPropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}