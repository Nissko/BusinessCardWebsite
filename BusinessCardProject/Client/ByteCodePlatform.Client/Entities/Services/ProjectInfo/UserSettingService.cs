using System.Text.Json;
using BusinessCardProject.Client.Entities.Services.ProjectInfo.Entity;
using Microsoft.JSInterop;

namespace BusinessCardProject.Client.Entities.Services.ProjectInfo
{
    /// <summary>
    /// Сервис пользовательских настроек в LocalStorage
    /// </summary>
    internal class UserSettingService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string StorageKey = "userSettings";

        public UserSettingsEntity Settings { get; private set; } = new();

        public event Action? OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();

        public UserSettingService(IJSRuntime js)
        {
            _jsRuntime = js;
        }

        /// <summary>
        /// Загрузка настроек из хранилища
        /// </summary>
        public async Task LoadAsync()
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
                if (!string.IsNullOrEmpty(json))
                {
                    Settings = JsonSerializer.Deserialize<UserSettingsEntity>(json) ?? new();
                }
            }
            catch
            {
                Settings = new();
            }
        }

        /// <summary>
        /// Сохранение изменений
        /// </summary>
        public async Task SaveAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(Settings);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
                OnChange?.Invoke();
            }
            catch
            {
                Console.WriteLine("Failed to save preferences");
            }
        }
    }
}