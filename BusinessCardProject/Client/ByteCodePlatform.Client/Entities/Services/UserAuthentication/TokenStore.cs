using Microsoft.JSInterop;

namespace BusinessCardProject.Client.Entities.Services.UserAuthentication
{
    public class TokenStore
    {
        private readonly IJSRuntime _jsRuntime;
        private string? _accessToken;
        private string? _refreshToken;
        private DateTime? _expiryTime;

        public TokenStore(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        public string? GetAccessToken() => _accessToken;

        public async Task InitializeAsync()
        {
            _accessToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            _refreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");

            if (long.TryParse(await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "tokenExpiry"),
                    out var expiry))
            {
                _expiryTime = DateTime.FromBinary(expiry);
            }
        }

        public async Task SetTokensAsync(string accessToken, string refreshToken, int expiresInSeconds)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _expiryTime = DateTime.UtcNow.AddSeconds(expiresInSeconds);

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", accessToken);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshToken);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "tokenExpiry",
                _expiryTime.Value.ToBinary().ToString());
        }

        public string? GetRefreshToken() => _refreshToken;

        public async Task ClearAsync()
        {
            _accessToken = null;
            _refreshToken = null;
            _expiryTime = null;

            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "tokenExpiry");
        }
    }
}