using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Entities.Services.UserAuthentication
{
    public class AuthenticationDelegatingHandler : DelegatingHandler, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuthenticationDelegatingHandler> _logger;
        private bool _disposed;

        public AuthenticationDelegatingHandler(
            IServiceProvider serviceProvider,
            ILogger<AuthenticationDelegatingHandler> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_disposed)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var tokenStore = _serviceProvider.GetRequiredService<TokenStore>();
            var navManager = _serviceProvider.GetRequiredService<NavigationManager>();
            var clientAuthService = _serviceProvider.GetRequiredService<ClientAuthenticationService>();

            var token = tokenStore.GetAccessToken();
            
            if (!string.IsNullOrEmpty(token))
            {
                //проверка токена на преждевременное истечение
                await clientAuthService.ValidateTokenRegular();
                
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            response.Dispose();

            var refreshToken = tokenStore.GetRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
            {
                await tokenStore.ClearAsync();
                navManager.NavigateTo("/login", forceLoad: true);
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            var refreshSuccess = await TryRefreshTokenAsync(refreshToken, cancellationToken);

            if (refreshSuccess)
            {
                token = tokenStore.GetAccessToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await base.SendAsync(request, cancellationToken);
            }

            await tokenStore.ClearAsync();
            navManager.NavigateTo("/login", forceLoad: true);

            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        private async Task<bool> TryRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("AuthRefreshClient");

            var channel = Grpc.Net.Client.GrpcChannel.ForAddress(httpClient.BaseAddress!,
                new Grpc.Net.Client.GrpcChannelOptions
                {
                    HttpClient = httpClient,
                    DisposeHttpClient = false
                });

            try
            {
                var refreshClient =
                    new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

                var response = await refreshClient.RefreshTokenAsync(
                    new AuthorizationService.Proto.RefreshTokenRequest { RefreshToken = refreshToken },
                    cancellationToken: cancellationToken);

                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    var tokenStore = _serviceProvider.GetRequiredService<TokenStore>();
                    await tokenStore.SetTokensAsync(
                        response.AccessToken,
                        response.RefreshToken,
                        response.ExpiresIn);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при попытке обновления токена");
                return false;
            }
            finally
            {
                channel?.Dispose();
            }
        }

        public new void Dispose()
        {
            _disposed = true;
        }
    }
}