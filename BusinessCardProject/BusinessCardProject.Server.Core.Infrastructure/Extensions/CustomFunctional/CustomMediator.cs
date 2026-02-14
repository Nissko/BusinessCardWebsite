using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.CustomMediator;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessCardProject.Server.Core.Infrastructure.Extensions.CustomFunctional
{
    public class CustomMediator : ICustomMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle))!;
            return (TResponse)await (Task<TResponse>)method.Invoke(handler, [request, cancellationToken])!;
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(typeof(TNotification));
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var method = handlerType.GetMethod(nameof(INotificationHandler<TNotification>.Handle))!;
                await (Task)method.Invoke(handler, [notification, cancellationToken])!;
            }
        }
    }
}