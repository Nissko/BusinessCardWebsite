using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("UsersAuthService");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Surname)
                .HasMaxLength(30)
                .IsRequired()
                .HasComment("Фамилия");
        
            builder.Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired()
                .HasComment("Имя");
        
            builder.Property(x => x.NickName)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("Ник");
        
            builder.Property(x => x.Email)
                .HasMaxLength(50)
                .IsRequired()
                .HasComment("Почта");
        
            builder.Property(x=>x.PasswordHash)
                .IsRequired()
                .HasComment("Пароль");
        
            builder.Property(x => x.IsAuthor)
                .IsRequired()
                .HasComment("Является ли автором");
        
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasComment("Дата регистрации");
        
            builder.Property(x => x.UpdatedAt)
                .IsRequired(false)
                .HasComment("Дата изменения");
        
            builder.Property(x => x.DeletedAt)
                .IsRequired(false)
                .HasComment("Дата удаления");

            builder.Property(x => x.VerifyMail)
                .HasDefaultValue(false)
                .HasComment("Подтверждение аккаунта");

            builder.HasIndex(x => x.NickName).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}