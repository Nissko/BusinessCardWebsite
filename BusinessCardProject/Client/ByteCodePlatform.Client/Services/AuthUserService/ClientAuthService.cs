using AuthGrpcService.Proto;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace BusinessCardProject.Client.Services.AuthUserService
{
    public class ClientAuthService
    {
        private readonly TokenStore _tokenStore;

        public ClientAuthService(TokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        public async Task<bool> Login(string email, string password)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress("http://localhost:5012", new GrpcChannelOptions
            {
                HttpHandler = handler
            });

            var client = new AuthGrpcService.Proto.AuthGrpcService.AuthGrpcServiceClient(channel);

            try
            {
                var response = await client.LoginAsync(new LoginRequest { Email = email, Password = password });

                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    await _tokenStore.SetTokensAsync(response.AccessToken, response.RefreshToken, response.ExpiresIn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> RefreshToken(string refreshToken)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress("http://localhost:5012", new GrpcChannelOptions
            {
                HttpHandler = handler
            });

            var client = new AuthGrpcService.Proto.AuthGrpcService.AuthGrpcServiceClient(channel);

            try
            {
                var response = await client.RefreshTokenAsync(new RefreshTokenRequest { RefreshToken = refreshToken });

                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    await _tokenStore.SetTokensAsync(response.AccessToken, response.RefreshToken, response.ExpiresIn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
            }

            return false;
        }

        public async Task<bool> Logout(string? refreshToken)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress("http://localhost:5012", new GrpcChannelOptions
            {
                HttpHandler = handler
            });

            var client = new AuthGrpcService.Proto.AuthGrpcService.AuthGrpcServiceClient(channel);

            try
            {
                _ = await client.LogoutAsync(new LogoutRequest()
                {
                    RefreshToken = refreshToken
                });

                ClearToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
            }

            return false;
        }

        public string? GetToken() => _tokenStore.GetAccessToken();
        private void ClearToken() => _ = _tokenStore.ClearAsync();
    }
}