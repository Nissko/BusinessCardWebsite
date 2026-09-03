using ByteCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteCodePlatform.Infrastructure.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
    {
        public void Configure(EntityTypeBuilder<AuthorEntity> builder)
        {
            builder.ToTable("Authors");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Имя");

            builder.Property(x => x.Surname)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Фамилия");

            builder.Property(x => x.AboutUs)
                .IsRequired()
                .HasMaxLength(2000)
                .HasComment("Об авторе");

            builder.Property(x => x.AvatarId)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Аватар");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasComment("Дата регистрации");
        
            builder.Property(x => x.UpdatedAt)
                .IsRequired(false)
                .HasComment("Дата изменения");
        
            builder.Property(x => x.DeletedAt)
                .IsRequired(false)
                .HasComment("Дата удаления");

            builder.HasOne(x => x.User)
                .WithOne(x => x.Author)
                .HasForeignKey<AuthorEntity>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        
            builder.HasIndex(x => x.UserId).IsUnique();
        }
    }
}