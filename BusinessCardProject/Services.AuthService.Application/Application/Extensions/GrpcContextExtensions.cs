using System.Security.Claims;
using Grpc.Core;

namespace Services.AuthService.Application.Application.Extensions
{
    public static class GrpcContextExtensions
    {
        public static string? GetUserIdFromToken(this ServerCallContext context)
            => context.GetHttpContext()?.User?.FindFirst("sub")?.Value
               ?? context.GetHttpContext()?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        public static string GetClientIpAddress(this ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext?.Items["RealIpAddress"] is string ip && !string.IsNullOrEmpty(ip))
                return ip;

            return context.Peer?.Split(':')[0] ?? "unknown";
        }
    }
}