using Grpc.Core;
using Grpc.Core.Interceptors;

namespace ByteCodePlatform.Admin.Entities.Services.UserAuthentication
{
    public class AuthenticationInterceptor : Interceptor, IDisposable
    {
        private readonly TokenStore _tokenStore;
        private bool _disposed;

        public AuthenticationInterceptor(TokenStore tokenStore)
        {
            _tokenStore = tokenStore ?? throw new ArgumentNullException(nameof(tokenStore));
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            if (_disposed)
                return base.AsyncUnaryCall(request, context, continuation);

            var existingHeaders = new Metadata();
            
            if (context.Options.Headers != null)
            {
                foreach (var header in context.Options.Headers)
                {
                    if (!header.Key.Equals("authorization", StringComparison.OrdinalIgnoreCase))
                        existingHeaders.Add(header.Key, header.Value);
                }
            }

            var token = _tokenStore.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                existingHeaders.Add("authorization", $"Bearer {token}");
            }

            var newOptions = context.Options.WithHeaders(existingHeaders);

            var newContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method, context.Host, newOptions);

            return base.AsyncUnaryCall(request, newContext, continuation);
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}