using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Application.Common.Interfaces
{
    public interface IAuthDbContext
    {
        DatabaseFacade Database { get; }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<UserRolesEntity> UserRole { get; set; }
        public DbSet<RefreshTokenEntity> RefreshToken { get; set; }
        public DbSet<AccountActivationEntity> AccountVerification { get; set; }
        public DbSet<FailedLoginAttemptEntity> FailedLoginAttempts { get; set; }
        public DbSet<PasswordResetEntity> PasswordResets { get; set; }
        public DbSet<AuditLogEntity> AuditLogs { get; set; }
        public DbSet<UserSettingEntity> UserSettings { get; set; }

        void Migrate();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}