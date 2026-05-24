using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRolesEntity>
    {
        public void Configure(EntityTypeBuilder<UserRolesEntity> builder)
        {
            builder.ToTable("UserRoles");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RoleId)
                .IsRequired()
                .HasComment("Фамилия");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasComment("Фамилия");

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}