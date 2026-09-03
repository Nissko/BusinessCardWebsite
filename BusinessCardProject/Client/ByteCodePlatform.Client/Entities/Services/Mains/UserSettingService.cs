using System.Text.Json;
using AuthorizationService.Proto;
using BusinessCardProject.Client.Entities.Services.Mains.Entity;
using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.JSInterop;

namespace BusinessCardProject.Client.Entities.Services.Mains
{
    internal class UserSettingService : IDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient? _authGrpcServiceClient;
        private TokenStore? _tokenStore;
        private const string StorageKey = "userSettings";
        private bool _disposed;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = false
        };

        public UserSettingsEntity Settings { get; private set; } = new();

        public event Action? OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();

        public UserSettingService(IJSRuntime js)
        {
            _jsRuntime = js ?? throw new ArgumentNullException(nameof(js));
        }

        public void SetAuthorizationGrpcClient(
            AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient client) =>
            _authGrpcServiceClient = client;

        public void SetTokenStorage(TokenStore tokenStore) => _tokenStore = tokenStore;

        public Task Load() => LoadFromLocalStorage();

        private async Task LoadFromLocalStorage()
        {
            if (_disposed) return;

            try
            {
                var jsonSetting = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
                
                if (!string.IsNullOrEmpty(jsonSetting))
                {
                    Settings = JsonSerializer.Deserialize<UserSettingsEntity>(jsonSetting, JsonOptions) ?? new();
                }
                else
                {
                    Settings.UpdateTime = DateTime.UtcNow;
                    await SaveToLocalStorage();
                    NotifyStateChanged();
                }
            }
            catch (JsonException)
            {
                Settings = new() { UpdateTime = DateTime.UtcNow };
                await SaveToLocalStorage();
                NotifyStateChanged();
            }
            catch
            {
                // ignored
            }
        }

        public async Task Save()
        {
            if (_disposed) return;

            Settings.UpdateTime = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(Settings, JsonOptions);

            try
            {
                await SaveToLocalStorage(json);
                NotifyStateChanged();
            }
            catch
            {
                // ignored
            }

            if (_authGrpcServiceClient != null && _tokenStore != null && !string.IsNullOrEmpty(_tokenStore.GetAccessToken()))
            {
                await SaveToBackend(json);
            }
        }

        private Task SaveToLocalStorage()
        {
            var json = JsonSerializer.Serialize(Settings, JsonOptions);
            return SaveToLocalStorage(json);
        }

        private async Task SaveToLocalStorage(string json)
        {
            if (_disposed) return;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }

        public async Task SyncWithBackend()
        {
            if (_authGrpcServiceClient == null || _disposed) return;
    
            try
            {
                var response = await _authGrpcServiceClient.GetUserSettingsAsync(new GetUserSettingsRequest());
                if (!string.IsNullOrEmpty(response.JsonSettings))
                {
                    Settings = JsonSerializer.Deserialize<UserSettingsEntity>(response.JsonSettings, JsonOptions) ?? new();
                    await SaveToLocalStorage();
                    NotifyStateChanged();
                }
            }
            catch
            {
                // ignored
            }
        }

        private async Task SaveToBackend(string json)
        {
            if (_authGrpcServiceClient == null || _disposed) return;
    
            try
            {
                await _authGrpcServiceClient.SaveUserSettingsAsync(new UserSettingsRequest { JsonSettings = json });
            }
            catch
            {
                // ignored
            }
        }

        public void Dispose()
        {
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}