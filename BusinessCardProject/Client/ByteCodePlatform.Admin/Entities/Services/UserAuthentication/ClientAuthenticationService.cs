using AuthorizationService.Proto;
using Grpc.Core;
using Microsoft.AspNetCore.Components;

namespace ByteCodePlatform.Admin.Entities.Services.UserAuthentication
{
    public class ClientAuthenticationService
    {
        private readonly TokenStore _tokenStore;
        private readonly AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient _client;
        private readonly NavigationManager _navManager;
        private readonly IServiceProvider _serviceProvider;

        public ClientAuthenticationService(
            TokenStore tokenStore,
            NavigationManager navManager, IServiceProvider serviceProvider, AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient client)
        {
            _tokenStore = tokenStore ?? throw new ArgumentNullException(nameof(tokenStore));
            _navManager = navManager ?? throw new ArgumentNullException(nameof(navManager));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var response = await _client.LoginAsync(new LoginRequest { Email = email, Password = password });
                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    await _tokenStore.SetTokensAsync(response.AccessToken, response.RefreshToken, response.ExpiresIn);
                    return true;
                }
            }
            catch (RpcException)
            {
            }

            return false;
        }

        public async Task<bool> ValidateAndClearAsync()
        {
            if (!_tokenStore.IsExpired)
            {
                return true;
            }

            var refreshToken = _tokenStore.GetRefreshToken();
            if (string.IsNullOrEmpty(refreshToken))
            {
                await _tokenStore.ClearAsync();
                return false;
            }

            try
            {
                var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("AuthRefreshClient");

                var channel = Grpc.Net.Client.GrpcChannel.ForAddress(httpClient.BaseAddress!,
                    new Grpc.Net.Client.GrpcChannelOptions { HttpClient = httpClient });

                var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);
                var response = await client.ValidateRefreshTokenAsync(
                    new RefreshTokenRequest { RefreshToken = refreshToken });

                if (!response.IsValid)
                {
                    await _tokenStore.ClearAsync();
                    return false;
                }

                var refreshResponse = await client.RefreshTokenAsync(
                    new RefreshTokenRequest { RefreshToken = refreshToken });

                if (!string.IsNullOrEmpty(refreshResponse.AccessToken))
                {
                    await _tokenStore.SetTokensAsync(
                        refreshResponse.AccessToken,
                        refreshResponse.RefreshToken,
                        refreshResponse.ExpiresIn);
                    return true;
                }

                await _tokenStore.ClearAsync();
                return false;
            }
            catch
            {
                await _tokenStore.ClearAsync();
                return false;
            }
        }

        public string? GetToken() => _tokenStore.GetAccessToken();
    }
}