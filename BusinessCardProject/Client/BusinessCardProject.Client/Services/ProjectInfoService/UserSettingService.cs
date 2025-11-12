using System.Text.Json;
using BusinessCardProject.Client.Services.ProjectInfoService.Classes;
using Microsoft.JSInterop;

namespace BusinessCardProject.Client.Services.ProjectInfoService;

/// <summary>
/// Сервис пользовательских настроек в LocalStorage
/// </summary>
public class UserSettingService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "userSettings";

    public UserSettingsClass Settings { get; private set; } = new();

    public event Action? OnChange;

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
                Settings = JsonSerializer.Deserialize<UserSettingsClass>(json) ?? new();
            }
        }
        catch (Exception ex)
        {
            // Логирование ошибки (если есть логгер)
            Console.WriteLine($"Failed to load preferences: {ex.Message}");
            Settings = new(); // fallback
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
            OnChange?.Invoke(); // уведомить компоненты об обновлении
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save preferences: {ex.Message}");
        }
    }
}