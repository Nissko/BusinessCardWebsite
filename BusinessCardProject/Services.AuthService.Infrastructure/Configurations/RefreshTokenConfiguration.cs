using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(x => x.TokenHash);

            builder.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(64)
                .ValueGeneratedNever();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired()
                .HasComment("Дата создания");

            builder.Property(x => x.ExpiresAtUtc)
                .IsRequired()
                .HasComment("Дата истечения");

            builder.Property(x => x.IsRevoked)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_RefreshTokens_UserId");

            builder.HasIndex(x => new { x.UserId, x.IsRevoked })
                .HasDatabaseName("IX_RefreshTokens_UserId_IsRevoked")
                .IsUnique(false);
        }
    }
}