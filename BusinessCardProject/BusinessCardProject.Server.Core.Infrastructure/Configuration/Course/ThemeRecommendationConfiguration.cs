using BusinessCardProject.Server.Core.Domain.Aggregates.Course.CourseTheme;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.Course
{
    public class ThemeRecommendationConfiguration : IEntityTypeConfiguration<ThemeRecommendationEntity>
    {
        public void Configure(EntityTypeBuilder<ThemeRecommendationEntity> builder)
        {
            builder.ToTable("ThemeRecommendations");
            builder.HasKey(x => x.Id);
        
            builder.Property<string>("_title")
                .HasColumnName("Title")
                .HasMaxLength(200)
                .IsRequired()
                .HasComment("Рекомендация");
        
            builder.HasOne(x=>x.CourseTheme)
                .WithMany(x=>x.ThemeRecommendations)
                .HasForeignKey(x=>x.CourseThemeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}