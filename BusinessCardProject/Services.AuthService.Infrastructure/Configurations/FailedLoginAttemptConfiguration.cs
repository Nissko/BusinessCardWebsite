using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Configurations
{
    public class FailedLoginAttemptConfiguration : IEntityTypeConfiguration<FailedLoginAttemptEntity>
    {
        public void Configure(EntityTypeBuilder<FailedLoginAttemptEntity> builder)
        {
            builder.ToTable("FailedLoginAttempts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasComment("Идентификатор пользователя");

            builder.Property(x => x.AttemptedAt)
                .IsRequired()
                .HasComment("Время попытки входа");

            builder.HasIndex(x => new { x.UserId, x.AttemptedAt })
                .HasDatabaseName("IX_FailedLoginAttempts_UserId_AttemptedAt");

            builder.HasIndex(x => x.AttemptedAt)
                .HasDatabaseName("IX_FailedLoginAttempts_AttemptedAt");
        }
    }
}