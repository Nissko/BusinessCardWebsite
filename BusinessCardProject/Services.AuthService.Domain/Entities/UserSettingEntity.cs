using NodaTime;

namespace Services.AuthService.Domain.Entities
{
    public class UserSettingEntity
    {
        public UserSettingEntity(Guid userId, string jsonSettings = "")
        {
            UserId = userId;
            JsonSettings = jsonSettings;
            UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        }

        public Guid UserId { get; private set; }
        public string JsonSettings { get; private set; }
        public Instant UpdatedAt { get; private set; }

        public void UpdateJsonSettings(string jsonSettings)
        {
            JsonSettings = string.IsNullOrEmpty(jsonSettings) ? JsonSettings : jsonSettings;
            UpdatedAt = SystemClock.Instance.GetCurrentInstant();
        }
    }
}