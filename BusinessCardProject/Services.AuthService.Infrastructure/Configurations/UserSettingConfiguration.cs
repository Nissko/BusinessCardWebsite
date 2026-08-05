using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Configurations
{
    public class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettingEntity>
    {
        public void Configure(EntityTypeBuilder<UserSettingEntity> builder)
        {
            builder.ToTable("UserSettings");

            builder.HasKey(us => us.UserId);

            builder.Property(us => us.UserId)
                .ValueGeneratedOnAdd();

            builder.Property(us => us.JsonSettings)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(us => us.UpdatedAt)
                .IsRequired();
        }
    }
}