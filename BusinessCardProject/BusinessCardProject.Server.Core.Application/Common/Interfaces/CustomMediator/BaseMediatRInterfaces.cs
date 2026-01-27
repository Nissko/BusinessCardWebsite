namespace BusinessCardProject.Server.Core.Application.Common.Interfaces.CustomMediator;


public interface IRequest<out TResponse> { }

public interface IRequest : IRequest<Unit> { }

public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken ct);
}

public interface INotification { }

public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken ct);
}

public readonly struct Unit
{
    public static readonly Unit Value = new();
}