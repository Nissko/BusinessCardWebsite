using System.Text.Json;
using AuthorizationService.Proto;
using BusinessCardProject.Client.Entities.Services.ProjectInfo.Entity;
using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.JSInterop;

namespace BusinessCardProject.Client.Entities.Services.ProjectInfo
{
    internal class UserSettingService
    {
        private readonly IJSRuntime _jsRuntime;
        private AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient? _authGrpcServiceClient;
        private TokenStore _tokenStore;
        private const string StorageKey = "userSettings";

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

        public Task LoadAsync() => LoadFromLocalStorageAsync();

        private async Task LoadFromLocalStorageAsync()
        {
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
                    await SaveToLocalStorageAsync();
                    NotifyStateChanged();
                }
            }
            catch (JsonException)
            {
                Settings = new() { UpdateTime = DateTime.UtcNow };
                await SaveToLocalStorageAsync();
                NotifyStateChanged();
            }
            catch
            {
                // ignored
            }
        }

        public async Task SaveAsync()
        {
            Settings.UpdateTime = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(Settings, JsonOptions);

            try
            {
                await SaveToLocalStorageAsync(json);
                NotifyStateChanged();
            }
            catch
            {
                // ignored
            }

            if (_authGrpcServiceClient != null && !string.IsNullOrEmpty(_tokenStore.GetAccessToken()))
            {
                await SaveToBackendAsync(json);
            }
        }

        private Task SaveToLocalStorageAsync()
        {
            var json = JsonSerializer.Serialize(Settings, JsonOptions);
            return SaveToLocalStorageAsync(json);
        }

        private async Task SaveToLocalStorageAsync(string json)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }

        public async Task SyncWithBackendAsync()
        {
            if (_authGrpcServiceClient == null) return;
    
            try
            {
                var response = await _authGrpcServiceClient.GetUserSettingsAsync(new GetUserSettingsRequest());
                if (!string.IsNullOrEmpty(response.JsonSettings))
                {
                    Settings = JsonSerializer.Deserialize<UserSettingsEntity>(response.JsonSettings, JsonOptions) ?? new();
                    await SaveToLocalStorageAsync();
                    NotifyStateChanged();
                }
            }
            catch
            {
                // ignored
            }
        }

        private async Task SaveToBackendAsync(string json)
        {
            if (_authGrpcServiceClient == null) return;
    
            try
            {
                await _authGrpcServiceClient.SaveUserSettingsAsync(new UserSettingsRequest { JsonSettings = json });
            }
            catch
            {
                // ignored
            }
        }
    }
}