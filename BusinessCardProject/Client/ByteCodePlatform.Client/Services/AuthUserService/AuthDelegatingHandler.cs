using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BusinessCardProject.Client.Services.AuthUserService
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly TokenStore _tokenStore;
        private readonly NavigationManager _navManager;
        private readonly ClientAuthService _authService;
        private readonly SemaphoreSlim _refreshLock = new(1, 1);

        public AuthDelegatingHandler(TokenStore tokenStore, NavigationManager navManager, IJSRuntime jsRuntime,
            ClientAuthService authService)
        {
            _tokenStore = tokenStore;
            _navManager = navManager;
            _authService = authService;
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await AddAuthHeaderAsync(request);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Dispose();

                var refreshSuccess = await TryRefreshTokenAsync(cancellationToken);

                if (refreshSuccess)
                {
                    await AddAuthHeaderAsync(request);
                    return await base.SendAsync(request, cancellationToken);
                }

                await _tokenStore.ClearAsync();
                _navManager.NavigateTo("/login", forceLoad: true);
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            return response;
        }

        private Task AddAuthHeaderAsync(HttpRequestMessage request)
        {
            try
            {
                var token = _tokenStore.GetAccessToken();
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = null;
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                return Task.FromException(exception);
            }
        }

        private async Task<bool> TryRefreshTokenAsync(CancellationToken cancellationToken)
        {
            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                var refreshToken = _tokenStore.GetRefreshToken();
                if (string.IsNullOrEmpty(refreshToken))
                    return false;

                var response = await _authService.RefreshToken(refreshToken);
                return response;
            }
            catch
            {
                return false;
            }
            finally
            {
                _refreshLock.Release();
            }
        }
    }
}