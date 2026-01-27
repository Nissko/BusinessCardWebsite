using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.Course;

public class CourseAuthorEntityConfiguration : IEntityTypeConfiguration<CourseAuthorEntity>
{
    public void Configure(EntityTypeBuilder<CourseAuthorEntity> builder)
    {
        builder.ToTable("CourseAuthors");
        builder.HasKey(x => x.Id);

        builder.Property<string>("_authorName")
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property<string>("_authorSurname")
            .HasColumnName("Surname")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property<string>("_authorPatronymic")
            .HasColumnName("Patronymic")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property<string>("_authorNickName")
            .HasColumnName("Nickname")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex("_authorNickName").IsUnique();
    }
}