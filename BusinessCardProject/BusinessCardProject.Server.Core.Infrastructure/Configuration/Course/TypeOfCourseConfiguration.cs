using BusinessCardProject.Server.Core.Domain.Enums.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.Course;

public class TypeOfCourseConfiguration : IEntityTypeConfiguration<TypeOfCourseEnum>
{
    public void Configure(EntityTypeBuilder<TypeOfCourseEnum> builder)
    {
        builder.ToTable("TypeOfCourses");

        builder.Property(o => o.Id)
            .ValueGeneratedNever();

        builder.Property(o => o.Name)
            .HasMaxLength(200)
            .HasComment("Название типа курса");

        builder.HasIndex(x => x.Id);
    }
}