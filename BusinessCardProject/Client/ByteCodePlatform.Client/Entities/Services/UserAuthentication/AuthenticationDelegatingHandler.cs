using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Entities.Services.UserAuthentication
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuthenticationDelegatingHandler> _logger;
        private readonly SemaphoreSlim _refreshLock = new(1, 1);

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
            var tokenStore = _serviceProvider.GetRequiredService<TokenStore>();
            var navManager = _serviceProvider.GetRequiredService<NavigationManager>();

            var token = tokenStore.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Dispose();

                var refreshToken = tokenStore.GetRefreshToken();
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var isValid = await ValidateRefreshTokenAsync(refreshToken, cancellationToken);
                    if (!isValid)
                    {
                        await tokenStore.ClearAsync();
                        navManager.NavigateTo("/login", forceLoad: true);
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }
                }

                var refreshSuccess = await TryRefreshTokenAsync(cancellationToken);

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

            return response;
        }

        private async Task<bool> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("AuthRefreshClient");

                var channel = Grpc.Net.Client.GrpcChannel.ForAddress(httpClient.BaseAddress!,
                    new Grpc.Net.Client.GrpcChannelOptions
                    {
                        HttpClient = httpClient
                    });

                var validateClient =
                    new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

                var response = await validateClient.ValidateRefreshTokenAsync(
                    new AuthorizationService.Proto.RefreshTokenRequest { RefreshToken = refreshToken },
                    cancellationToken: cancellationToken);

                return response.IsValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при валидации refresh токена");
                return false;
            }
        }

        private async Task<bool> TryRefreshTokenAsync(CancellationToken cancellationToken)
        {
            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                var tokenStore = _serviceProvider.GetRequiredService<TokenStore>();
                var refreshToken = tokenStore.GetRefreshToken();
                if (string.IsNullOrEmpty(refreshToken)) return false;

                var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("AuthRefreshClient");

                var channel = Grpc.Net.Client.GrpcChannel.ForAddress(httpClient.BaseAddress!,
                    new Grpc.Net.Client.GrpcChannelOptions
                    {
                        HttpClient = httpClient
                    });

                var refreshClient =
                    new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

                var response = await refreshClient.RefreshTokenAsync(
                    new AuthorizationService.Proto.RefreshTokenRequest { RefreshToken = refreshToken },
                    cancellationToken: cancellationToken);

                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    await tokenStore.SetTokensAsync(response.AccessToken, response.RefreshToken, response.ExpiresIn);
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
                _refreshLock.Release();
            }
        }
    }
}