using BusinessCardProject.Server.Core.Domain.Aggregates.User;
using BusinessCardProject.Server.Core.Domain.Enums.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.User;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable("User_Roles");
        builder.HasKey(o => o.Id);

        builder.Property(x => x.UserRole)
            .HasColumnName("RoleId")
            .HasConversion(
                v => v.Id,
                v => UserRoleEnum.List().First(t => t.Id == v))
            .HasComment("Ид роли пользователя");

        builder.HasOne(x => x.UserProfile)
            .WithMany(x => x.UserRoles)
            .HasForeignKey("UserProfileId")
            .OnDelete(DeleteBehavior.SetNull);
    }
}