using System.Text.Json;
using BusinessCardProject.Server.Core.Domain.Aggregates.User;
using BusinessCardProject.Server.Core.Domain.Aggregates.User.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.User
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
    {
        public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
        {
            builder.ToTable("UserProfiles");
            builder.HasKey(x => x.Id);

            builder.Property<string>(x => x.Surname)
                .HasColumnName("Surname")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("Фамилия");

            builder.Property<string>(x => x.Name)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("Имя");

            builder.Property<string>(x => x.Patronymic)
                .HasColumnName("Patronymic")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("Отчество");

            builder.Property<string>(x => x.Email)
                .HasColumnName("Email")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("Почта");

            builder.Property<string>(x => x.AltName)
                .HasColumnName("NickName")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("Почта");

            builder.Property<string>("_password")
                .HasColumnName("Password")
                .HasMaxLength(100)
                .IsRequired()
                .HasComment("Пароль");

            builder.Property(x => x.DateOfRegistered)
                .HasColumnName("DateOfRegistered")
                .HasComment("Дата регистрации");

            builder.Property(x => x.DateOfDeletion)
                .HasColumnName("DateOfDeletion")
                .IsRequired(false)
                .HasComment("Дата деактивации аккаунта");

            builder.Property<bool>("_isActive")
                .HasColumnName("isActive")
                .HasDefaultValue(false)
                .IsRequired()
                .HasComment("Признак работоспособности профиля");

            builder.Property<bool>("_isBlocked")
                .HasColumnName("isBlocked")
                .HasDefaultValue(false)
                .IsRequired()
                .HasComment("Признак блокировки");

            builder.Property(e => e.Settings)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<UserSetting>(v, JsonSerializerOptions.Default) ?? new UserSetting()
                )
                .HasComment("Настройки пользователя");

            builder.HasIndex(x => x.AltName).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}