using ByteCodePlatform.Domain.Entities.Course.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteCodePlatform.Infrastructure.Configuration.FieldProperties
{
    public class CourseContentFieldPropertyConfiguration : IEntityTypeConfiguration<CourseContentFieldPropertyEntity>
    {
        public void Configure(EntityTypeBuilder<CourseContentFieldPropertyEntity> builder)
        {
            builder.ToTable("CourseContentFieldProperties");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                .IsRequired()
                .HasComment("Значение");
        
            builder.HasOne(x => x.CourseContent)
                .WithMany(x => x.ContentFieldProperties)
                .HasForeignKey(x => x.CourseContentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(x=>x.FieldPropertyType)
                .WithMany(x => x.CourseContent)
                .HasForeignKey(x => x.FieldPropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}