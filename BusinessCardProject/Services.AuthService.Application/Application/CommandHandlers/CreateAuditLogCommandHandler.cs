using MediatR;
using Services.AuthService.Application.Application.Command;
using Services.AuthService.Application.Application.Extensions;
using Services.AuthService.Application.Common.Interfaces;

namespace Services.AuthService.Application.Application.CommandHandlers
{
    public class CreateAuditLogCommandHandler : IRequestHandler<CreateAuditLogCommand>
    {
        private readonly IAuditLogRepository _auditLog;

        public CreateAuditLogCommandHandler(IAuditLogRepository auditLog)
        {
            _auditLog = auditLog ?? throw new ArgumentNullException(nameof(auditLog));
        }

        public async Task<Unit> Handle(CreateAuditLogCommand request, CancellationToken cancellationToken)
        {
            var userIpAddress = request.Context.GetClientIpAddress();
            var userAgent = request.Context.RequestHeaders
                .FirstOrDefault(x => x.Key.Equals("user-agent", StringComparison.OrdinalIgnoreCase))?.Value ?? "";
            var exceptionMessage = request.Exception?.Message;
            var detailsLog = GetDetailLog(request.Action, userIpAddress, exceptionMessage);

            await _auditLog.Log(request.UserId, request.Action, detailsLog, userIpAddress, userAgent,
                cancellationToken);

            return Unit.Value;
        }

        private static string GetDetailLog(string action, string ip, string? exMessage = "")
        {
            return action switch
            {
                "Login" => $"Успещный вход с IP - {ip}",
                "VerificationAccount" => string.IsNullOrEmpty(exMessage)
                    ? $"Успешное подтверждение аккаунта с IP - {ip}"
                    : $"Произошла ошибка при подтверждении аккаунта с IP - {ip}. Ошибка {exMessage}",
                _ => ""
            };
        }
    }
}