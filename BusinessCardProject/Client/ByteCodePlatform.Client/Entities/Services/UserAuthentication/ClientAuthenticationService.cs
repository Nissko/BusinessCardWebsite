using AuthorizationService.Proto;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace BusinessCardProject.Client.Entities.Services.UserAuthentication
{
    public class ClientAuthenticationService
    {
        private readonly TokenStore _tokenStore;
        private const string AddressLink = "https://localhost:7241";
        //private const string AddressLink = "https://it-bytecode.splinterkeenetic.netcraze.club/AuthGrpcService";

        public ClientAuthenticationService(TokenStore tokenStore)
        {
            _tokenStore = tokenStore ?? throw new ArgumentNullException(nameof(tokenStore));
        }

        public async Task<bool> Login(string email, string password)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(AddressLink,
                new GrpcChannelOptions
                {
                    HttpHandler = handler
                });

            var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

            try
            {
                var response = await client.LoginAsync(new LoginRequest { Email = email, Password = password });

                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    await _tokenStore.SetTokensAsync(response.AccessToken, response.RefreshToken, response.ExpiresIn);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public async Task<bool> RefreshToken(string refreshToken)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(AddressLink,
                new GrpcChannelOptions
                {
                    HttpHandler = handler
                });

            var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

            try
            {
                var response = await client.RefreshTokenAsync(new RefreshTokenRequest { RefreshToken = refreshToken });

                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    await _tokenStore.SetTokensAsync(response.AccessToken, response.RefreshToken, response.ExpiresIn);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public async Task<bool> Logout(string? refreshToken)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(AddressLink,
                new GrpcChannelOptions
                {
                    HttpHandler = handler
                });

            var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

            try
            {
                _ = await client.LogoutAsync(new LogoutRequest()
                {
                    RefreshToken = refreshToken
                });

                ClearToken();
            }
            catch
            {
                return false;
            }

            return false;
        }
        
        public static async Task<bool> VerificationAccount(Guid userId, string verificationCode)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(AddressLink,
                new GrpcChannelOptions { HttpHandler = handler });

            var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

            try
            {
                var response = await client.VerificationAccountAsync(new VerificationAccountRequest
                {
                    UserId = userId.ToString(),
                    VerificationCode = verificationCode
                });

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public string? GetToken() => _tokenStore.GetAccessToken();
        private void ClearToken() => _ = _tokenStore.ClearAsync();
    }
}