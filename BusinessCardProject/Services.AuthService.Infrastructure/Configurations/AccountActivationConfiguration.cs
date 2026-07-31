using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Configurations
{
    public class AccountActivationConfiguration : IEntityTypeConfiguration<AccountActivationEntity>
    {
        public void Configure(EntityTypeBuilder<AccountActivationEntity> builder)
        {
            builder.ToTable("AccountActivationRecords");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.VerificationToken)
                .IsRequired()
                .HasComment("Токен верификации");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasComment("Время создания записи");

            builder.Property(x => x.ExpiresAt)
                .IsRequired()
                .HasComment("Время истечения срока");

            builder.Property(x => x.VerificationAt)
                .IsRequired(false)
                .HasComment("Время подтверждения");

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserVerifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.VerificationToken).IsUnique();

            builder.HasIndex(x => x.ExpiresAt)
                .HasDatabaseName("IX_AccountActivationRecords_ExpiresAt");

            builder.HasIndex(x => new { x.UserId, x.ExpiresAt })
                .HasDatabaseName("IX_AccountActivationRecords_UserId_ExpiresAt");
        }
    }
}