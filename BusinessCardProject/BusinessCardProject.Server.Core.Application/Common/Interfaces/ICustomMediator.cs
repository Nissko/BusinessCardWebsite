using BusinessCardProject.Server.Core.Application.Common.Interfaces.CustomMediator;

namespace BusinessCardProject.Server.Core.Application.Common.Interfaces
{
    public interface ICustomMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
        Task Publish<TNotification>(TNotification notification, CancellationToken ct = default) 
            where TNotification : INotification;
    }
}