using AuthorizationService.Proto;
using BusinessCardProject.Client.Pages;
using Grpc.Core;
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

        public static async Task<bool> SendPasswordReset(string email)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(AddressLink,
                new GrpcChannelOptions { HttpHandler = handler });

            var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

            try
            {
                var response = await client.SendPasswordResetAsync(new SendPasswordResetRequest { Email = email });
                return response.Success;
            }
            catch
            {
                return false;
            }
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
        
        public static async Task<bool> ResetPassword(Guid userId, string resetToken, string newPassword)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(AddressLink,
                new GrpcChannelOptions { HttpHandler = handler });

            var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

            try
            {
                var response = await client.ResetPasswordAsync(new ResetPasswordRequest
                {
                    UserId = userId.ToString(),
                    ResetToken = resetToken,
                    NewPassword = newPassword
                });
                return response.Success;
            }
            catch (RpcException ex) when (ex.Status.StatusCode == StatusCode.ResourceExhausted)
            {
                throw new Exception("Аккаунт временно заблокирован. Попробуйте позже");
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<SessionInfo>> GetActiveSessions(int limit)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(AddressLink,
                new GrpcChannelOptions { HttpHandler = handler });

            var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

            try
            {
                var response = await client.GetActiveSessionsAsync(new GetActiveSessionsRequest
                {
                    UserId = _tokenStore.GetAccessToken() ?? "",
                    Limit = limit
                });

                return response.Sessions.Select(s => new SessionInfo(
                    s.TokenId,
                    s.CreatedAt.ToDateTime(),
                    s.ExpiresAt.ToDateTime())).ToList();
            }
            catch
            {
                return new List<SessionInfo>();
            }
        }

        public static async Task<bool> RevokeSession(string tokenId)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(AddressLink,
                new GrpcChannelOptions { HttpHandler = handler });

            var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

            try
            {
                await client.LogoutAsync(new LogoutRequest { RefreshToken = tokenId });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LogoutAll()
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
            var channel = GrpcChannel.ForAddress(AddressLink,
                new GrpcChannelOptions { HttpHandler = handler });

            var client = new AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient(channel);

            try
            {
                await client.LogoutAllAsync(new LogoutAllRequest());
                ClearToken();
                return true;
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