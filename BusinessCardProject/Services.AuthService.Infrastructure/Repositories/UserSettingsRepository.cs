using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Infrastructure.Repositories
{
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly IAuthDbContext _context;

        public UserSettingsRepository(IAuthDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserSettingEntity?> GetByUserId(Guid userId)
        {
            return await _context.UserSettings
                .FindAsync([userId]);
        }

        public async Task Save(Guid userId, string jsonSettings)
        {
            var settings = await _context.UserSettings
                .FindAsync([userId]);

            if (settings == null)
            {
                settings = new UserSettingEntity(userId);
                _context.UserSettings.Add(settings);
            }

            settings.UpdateJsonSettings(jsonSettings);

            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}