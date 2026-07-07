using ByteCodePlatform.Domain.Entities.Course.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteCodePlatform.Infrastructure.Configuration.FieldProperties
{
    public class CourseModuleFieldPropertyConfiguration : IEntityTypeConfiguration<CourseModuleFieldPropertyEntity>
    {
        public void Configure(EntityTypeBuilder<CourseModuleFieldPropertyEntity> builder)
        {
            builder.ToTable("CourseModuleFieldProperties");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                .IsRequired()
                .HasComment("Значение");
        
            builder.HasOne(x => x.CourseModule)
                .WithMany(x => x.ModuleFieldProperties)
                .HasForeignKey(x => x.CourseModuleId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(x=>x.FieldPropertyType)
                .WithMany(x => x.CourseModule)
                .HasForeignKey(x => x.FieldPropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}