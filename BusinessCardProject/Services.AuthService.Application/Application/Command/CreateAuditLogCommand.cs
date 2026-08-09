using Grpc.Core;
using MediatR;

namespace Services.AuthService.Application.Application.Command
{
    public class CreateAuditLogCommand : IRequest
    {
        public CreateAuditLogCommand(Guid userId, string action, ServerCallContext context, Exception? exception = null)
        {
            UserId = userId;
            Action = action;
            Context = context;
            Exception = exception;
        }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Действие
        /// </summary>
        public string Action { get; private set; }
        
        /// <summary>
        /// Контекст вызова из gRPC
        /// </summary>
        public ServerCallContext Context { get; private set; }
        
        /// <summary>
        /// Ошибка по трейсу
        /// </summary>
        public Exception? Exception { get; private set; }
    }
}