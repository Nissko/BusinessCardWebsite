using System.Text.Json.Serialization;

namespace BusinessCardProject.Client.Entities.Services.Mains.Options
{
    /// <summary>
    /// Разные пункты настроек пользователя из страницы "Настройки"
    /// </summary>
    internal class UserSettingsList
    {
        private int _selectedPlatform;

        [JsonInclude] public int Platform => _selectedPlatform;

        [JsonConstructor]
        public UserSettingsList(int platform = 0)
        {
            _selectedPlatform = platform;
        }

        public UserSettingsList()
        {
        }

        public void SetPlatform(int platform)
        {
            _selectedPlatform = platform;
        }
    }
}