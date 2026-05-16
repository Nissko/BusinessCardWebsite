using ByteCodePlatform.Domain.Entities.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteCodePlatform.Infrastructure.Configuration
{
    public class ProgrammingLanguageCategoryConfiguration : IEntityTypeConfiguration<ProgrammingLanguageCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<ProgrammingLanguageCategoryEntity> builder)
        {
            builder.ToTable("ProgrammingLanguageCategories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(300)
                .IsRequired()
                .HasComment("Название модуля");
        }
    }
}