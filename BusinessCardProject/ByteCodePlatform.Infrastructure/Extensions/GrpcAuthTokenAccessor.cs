using ByteCodePlatform.Infrastructure.Extensions.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ByteCodePlatform.Infrastructure.Extensions
{
    public class GrpcAuthTokenAccessor : IAuthTokenAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GrpcAuthTokenAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return null;

            var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                return authHeader.Substring("Bearer ".Length);
            }

            return null;
        }
    }
}