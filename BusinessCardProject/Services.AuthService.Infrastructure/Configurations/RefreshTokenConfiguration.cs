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

            builder.Property(x => x.UserAgent)
                .IsRequired(false)
                .HasMaxLength(512);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.IsRevoked })
                .IsUnique(false);
            builder.HasIndex(x => new { x.UserId, x.UserAgent });
            builder.HasIndex(x => x.ExpiresAtUtc);
        }
    }
}