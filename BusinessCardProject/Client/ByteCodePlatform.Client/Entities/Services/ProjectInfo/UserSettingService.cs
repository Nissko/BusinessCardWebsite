using System.Text.Json;
using AuthorizationService.Proto;
using BusinessCardProject.Client.Entities.Services.ProjectInfo.Entity;
using BusinessCardProject.Client.Entities.Services.UserAuthentication;
using Microsoft.JSInterop;

namespace BusinessCardProject.Client.Entities.Services.ProjectInfo
{
    /// <summary>
    /// Сервис пользовательских настроек в LocalStorage
    /// </summary>
    internal class UserSettingService
    {
        private readonly IJSRuntime _jsRuntime;

        private readonly AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient
            _authGrpcServiceClient;

        private readonly ClientAuthenticationService _authService;
        private const string StorageKey = "userSettings";

        public UserSettingsEntity Settings { get; private set; } = new();

        public event Action? OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();

        public UserSettingService(
            IJSRuntime js,
            AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient authGrpcServiceClient,
            ClientAuthenticationService authService)
        {
            _jsRuntime = js ?? throw new ArgumentNullException(nameof(js));
            _authGrpcServiceClient =
                authGrpcServiceClient ?? throw new ArgumentNullException(nameof(authGrpcServiceClient));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public async Task LoadAsync()
        {
            var isAuthenticated = !string.IsNullOrEmpty(_authService.GetToken());

            if (isAuthenticated)
            {
                await LoadFromBackendAsync();
            }

            await LoadFromLocalStorageAsync();
        }

        private async Task LoadFromBackendAsync()
        {
            try
            {
                var response = await _authGrpcServiceClient.GetUserSettingsAsync(new GetUserSettingsRequest
                {
                    AccessToken = _authService.GetAccessToken()
                });

                if (!string.IsNullOrEmpty(response.JsonSettings))
                {
                    Settings = JsonSerializer.Deserialize<UserSettingsEntity>(response.JsonSettings) ?? new();
                }
            }
            catch
            {
                // ignored
            }
        }

        private async Task LoadFromLocalStorageAsync()
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
                if (!string.IsNullOrEmpty(json))
                {
                    var localSettings = JsonSerializer.Deserialize<UserSettingsEntity>(json);
                    if (localSettings != null)
                    {
                        Settings.IsDark = localSettings.IsDark;
                        Settings.IsDrawerOpen = localSettings.IsDrawerOpen;
                        Settings.VideoPlatform = localSettings.VideoPlatform;
                        Settings.ProgrammingLanguage = localSettings.ProgrammingLanguage;
                        Settings.UpdateTime ??= localSettings.UpdateTime;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public async Task SaveAsync()
        {
            await SaveToLocalStorageAsync();

            var isAuthenticated = !string.IsNullOrEmpty(_authService.GetToken());
            if (isAuthenticated)
            {
                await SaveToBackendAsync();
            }
        }

        private async Task SaveToBackendAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(Settings);
                await _authGrpcServiceClient.SaveUserSettingsAsync(new UserSettingsRequest
                {
                    AccessToken = _authService.GetAccessToken(),
                    JsonSettings = json
                });
            }
            catch
            {
                // If backend save fails, localStorage still has the settings
            }
        }

        private async Task SaveToLocalStorageAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(Settings);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
                Settings.UpdateTime = DateTime.UtcNow;
                OnChange?.Invoke();
            }
            catch
            {
                Console.WriteLine("Failed to save preferences");
            }
        }
    }
}