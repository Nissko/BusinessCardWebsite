using BusinessCardProject.Server.Core.Domain.Enums.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCardProject.Server.Core.Infrastructure.Configuration.User
{
    public class UserRoleEnumConfiguration : IEntityTypeConfiguration<UserRoleEnum>
    {
        public void Configure(EntityTypeBuilder<UserRoleEnum> builder)
        {
            builder.ToTable("TypeUserRoles");

            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .HasComment("Название роли");

            builder.HasIndex(x => x.Id);
        }
    }
}