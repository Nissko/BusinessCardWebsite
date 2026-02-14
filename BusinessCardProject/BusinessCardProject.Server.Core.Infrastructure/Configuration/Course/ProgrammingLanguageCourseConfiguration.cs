using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.Course
{
    public class ProgrammingLanguageCourseConfiguration : IEntityTypeConfiguration<ProgrammingLanguageCourseEntity>
    {
        public void Configure(EntityTypeBuilder<ProgrammingLanguageCourseEntity> builder)
        {
            builder.ToTable("ProgrammingLanguages");
            builder.HasKey(x => x.Id);

            builder.Property<string>("_name")
                .HasColumnName("Name")
                .IsRequired()
                .HasComment("Название языка программирования");

            builder.Property<int>("_countSelectedUser")
                .HasColumnName("CountSelectedUser")
                .HasComment("Количество людей использующий ЯП");
        }
    }
}