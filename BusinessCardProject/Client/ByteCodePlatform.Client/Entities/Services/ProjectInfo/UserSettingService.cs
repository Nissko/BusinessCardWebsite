using System.Text.Json;
using AuthorizationService.Proto;
using BusinessCardProject.Client.Entities.Services.ProjectInfo.Entity;
using Microsoft.JSInterop;

namespace BusinessCardProject.Client.Entities.Services.ProjectInfo
{
    internal class UserSettingService
    {
        private readonly IJSRuntime _jsRuntime;
        private AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient? _authGrpcServiceClient;
        private const string StorageKey = "userSettings";

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

        public async Task LoadAsync()
        {
            await LoadFromLocalStorageAsync();
        }

        private async Task LoadFromLocalStorageAsync()
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

        public async Task SaveAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(Settings);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
                Settings.UpdateTime = DateTime.UtcNow;
                OnChange?.Invoke();
            }
            catch { /* ignored */ }

            if (_authGrpcServiceClient != null)
            {
                await SaveToBackendAsync();
            }
        }

        public async Task SyncWithBackendAsync()
        {
            if (_authGrpcServiceClient == null) return;
    
            try
            {
                var response = await _authGrpcServiceClient.GetUserSettingsAsync(new GetUserSettingsRequest { });
                if (!string.IsNullOrEmpty(response.JsonSettings))
                {
                    Settings = JsonSerializer.Deserialize<UserSettingsEntity>(response.JsonSettings) ?? new();
                    OnChange?.Invoke();
                }
            }
            catch { /* ignored */ }
        }

        private async Task SaveToBackendAsync()
        {
            if (_authGrpcServiceClient == null) return;
    
            try
            {
                var json = JsonSerializer.Serialize(Settings);
                await _authGrpcServiceClient.SaveUserSettingsAsync(new UserSettingsRequest { JsonSettings = json });
            }
            catch { /* ignored */ }
        }
    }
}