using Grpc.Core;
using Grpc.Core.Interceptors;

namespace BusinessCardProject.Client.Services.AuthUserService
{
    public class AuthInterceptor : Interceptor
    {
        private readonly TokenStore _tokenStore;

        public AuthInterceptor(TokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var headers = context.Options.Headers ?? new Metadata();

            var token = _tokenStore.GetAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                headers.Add("authorization", $"Bearer {token}");
            }

            var newOptions = context.Options.WithHeaders(headers);
            var newContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method, context.Host, newOptions);

            return base.AsyncUnaryCall(request, newContext, continuation);
        }
    }
}