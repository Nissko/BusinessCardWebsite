using ByteCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteCodePlatform.Infrastructure.Configuration.FieldProperties
{
    public class FieldPropertyTypeConfiguration : IEntityTypeConfiguration<FieldPropertyTypeEntity>
    {
        public void Configure(EntityTypeBuilder<FieldPropertyTypeEntity> builder)
        {
            builder.ToTable("FieldPropertyTypes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasComment("Название свойства");
        
            builder.Property(x => x.Description)
                .IsRequired()
                .HasComment("Описание свойства");
        }
    }
}