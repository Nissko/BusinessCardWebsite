using Grpc.Core;
using MediatR;
using Services.AuthService.Application.Application.Command;
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
            var userIpAddress = GetRealIpAddress(request.Context);
            var userAgent = request.Context.RequestHeaders
                .FirstOrDefault(x => x.Key.Equals("user-agent", StringComparison.OrdinalIgnoreCase))?.Value ?? "";
            var exceptionMessage = request.Exception?.Message;
            var detailsLog = GetDetailLog(request.Action, userIpAddress, exceptionMessage);

            await _auditLog.Log(request.UserId, request.Action, detailsLog, userIpAddress, userAgent,
                cancellationToken);

            return Unit.Value;
        }

        private static string GetRealIpAddress(ServerCallContext context)
        {
            var xRealIp = context.RequestHeaders
                .FirstOrDefault(x => x.Key.Equals("x-real-ip", StringComparison.OrdinalIgnoreCase))?.Value;

            if (!string.IsNullOrEmpty(xRealIp))
                return xRealIp.Split(',')[0].Trim();

            var xForwardedFor = context.RequestHeaders
                .FirstOrDefault(x => x.Key.Equals("x-forwarded-for", StringComparison.OrdinalIgnoreCase))?.Value;

            if (!string.IsNullOrEmpty(xForwardedFor))
                return xForwardedFor.Split(',')[0].Trim();

            return context.Peer ?? "unknown";
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