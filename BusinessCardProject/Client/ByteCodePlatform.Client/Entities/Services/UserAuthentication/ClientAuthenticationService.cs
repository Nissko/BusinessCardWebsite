using AuthorizationService.Proto;
using BusinessCardProject.Client.Pages;
using Grpc.Core;
using Microsoft.AspNetCore.Components;

namespace BusinessCardProject.Client.Entities.Services.UserAuthentication
{
    public class ClientAuthenticationService
    {
        private readonly TokenStore _tokenStore;
        private readonly AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient _client;
        private readonly NavigationManager _navManager;
        private readonly IServiceProvider _serviceProvider;

        public ClientAuthenticationService(
            TokenStore tokenStore,
            AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient client,
            NavigationManager navManager, IServiceProvider serviceProvider)
        {
            _tokenStore = tokenStore ?? throw new ArgumentNullException(nameof(tokenStore));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _navManager = navManager ?? throw new ArgumentNullException(nameof(navManager));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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
            catch (RpcException) { }

            return false;
        }

        public async Task<bool> Logout()
        {
            var refreshToken = _tokenStore.GetRefreshToken();
            try
            {
                await _client.LogoutAsync(new LogoutRequest { RefreshToken = refreshToken });
            }
            catch (RpcException)
            {
            }
            finally
            {
                await _tokenStore.ClearAsync();
                _navManager.NavigateTo("/login", forceLoad: true);
            }

            return true;
        }

        /*TODO: доработать*/
        public async Task RevokeSession(string tokenId)
        {
            try
            {
                await _client.LogoutAsync(new LogoutRequest { RefreshToken = tokenId });
            }
            catch (RpcException)
            {
            }
        }

        public async Task<List<SessionInfo>> GetActiveSessions(int limit)
        {
            try
            {
                var accessToken = _tokenStore.GetAccessToken();
                if (string.IsNullOrEmpty(accessToken)) return new List<SessionInfo>();

                var response = await _client.GetActiveSessionsAsync(new GetActiveSessionsRequest
                {
                    AccessToken = accessToken,
                    Limit = limit
                });

                return response.Sessions.Select(s => new SessionInfo(
                    s.TokenId,
                    s.CreatedAt.ToDateTimeOffset().ToLocalTime(),
                    s.ExpiresAt.ToDateTimeOffset().ToLocalTime())).ToList();
            }
            catch (RpcException)
            {
                return new List<SessionInfo>();
            }
        }

        public async Task LogoutAll()
        {
            try
            {
                await _client.LogoutAllAsync(new LogoutAllRequest());
            }
            catch (RpcException)
            {
            }
            finally
            {
                await _tokenStore.ClearAsync();
                _navManager.NavigateTo("/login", forceLoad: true);
            }
        }

        public async Task<bool> SendPasswordReset(string email)
        {
            try
            {
                var response = await _client.SendPasswordResetAsync(new SendPasswordResetRequest { Email = email });
                return response.Success;
            }
            catch (RpcException)
            {
                return false;
            }
        }

        public async Task<bool> VerificationAccount(Guid userId, string verificationCode)
        {
            try
            {
                var response = await _client.VerificationAccountAsync(new VerificationAccountRequest
                {
                    UserId = userId.ToString(),
                    VerificationCode = verificationCode
                });
                return response.Success;
            }
            catch (RpcException)
            {
                return false;
            }
        }

        public async Task<bool> ResetPassword(Guid userId, string resetToken, string newPassword)
        {
            try
            {
                var response = await _client.ResetPasswordAsync(new ResetPasswordRequest
                {
                    UserId = userId.ToString(),
                    ResetToken = resetToken,
                    NewPassword = newPassword
                });
                return response.Success;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.ResourceExhausted)
            {
                throw new Exception("Аккаунт временно заблокирован. Попробуйте позже");
            }
            catch (RpcException)
            {
                return false;
            }
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

                var channel = Grpc.Net.Client.GrpcChannel.ForAddress(httpClient.BaseAddress,
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