using System.Net.Http.Headers;
using ByteCodePlatform.Infrastructure.Extensions.Interfaces;

namespace ByteCodePlatform.Infrastructure.Extensions
{
    public class AuthTokenPropagationHandler : DelegatingHandler
    {
        private readonly IAuthTokenAccessor _tokenAccessor;

        public AuthTokenPropagationHandler(IAuthTokenAccessor tokenAccessor)
        {
            _tokenAccessor = tokenAccessor ?? throw new ArgumentNullException(nameof(tokenAccessor));
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            var token = _tokenAccessor.GetToken();
        
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}