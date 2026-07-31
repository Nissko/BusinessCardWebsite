using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Configurations
{
    public class PasswordResetConfiguration : IEntityTypeConfiguration<PasswordResetEntity>
    {
        public void Configure(EntityTypeBuilder<PasswordResetEntity> builder)
        {
            builder.ToTable("PasswordResets");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasComment("Идентификатор пользователя");

            builder.Property(x => x.ResetToken)
                .IsRequired()
                .HasMaxLength(128)
                .HasComment("Токен сброса пароля");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasComment("Время создания токена");

            builder.Property(x => x.ExpiresAt)
                .IsRequired()
                .HasComment("Время истечения токена");

            builder.Property(x => x.IsUsed)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Использован ли токен");

            builder.HasIndex(x => new { x.UserId, x.IsUsed })
                .HasDatabaseName("IX_PasswordResets_UserId_IsUsed");

            builder.HasIndex(x => x.ExpiresAt)
                .HasDatabaseName("IX_PasswordResets_ExpiresAt");
        }
    }
}