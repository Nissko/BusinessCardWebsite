using Services.AuthService.Domain.Entities;

namespace Services.AuthService.Application.Common.Interfaces;

public interface IUserSettingsRepository
{
    /// <summary>
    /// 
    /// </summary>
    Task<UserSettingEntity?> GetByUserId(Guid userId);

    /// <summary>
    /// 
    /// </summary>
    Task Save(Guid userId, string jsonSettings);
}