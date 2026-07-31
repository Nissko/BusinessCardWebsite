using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLogEntity>
    {
        public void Configure(EntityTypeBuilder<AuditLogEntity> builder)
        {
            builder.ToTable("AuditLogs");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasComment("Идентификатор пользователя");

            builder.Property(x => x.Action)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Действие");

            builder.Property(x => x.Details)
                .HasMaxLength(500)
                .HasComment("Подробности");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasComment("Время записи");

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45)
                .HasComment("IP адрес");

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("User-Agent");

            builder.HasIndex(x => new { x.UserId, x.CreatedAt })
                .HasDatabaseName("IX_AuditLogs_UserId_CreatedAt");

            builder.HasIndex(x => x.CreatedAt)
                .HasDatabaseName("IX_AuditLogs_CreatedAt");
        }
    }
}