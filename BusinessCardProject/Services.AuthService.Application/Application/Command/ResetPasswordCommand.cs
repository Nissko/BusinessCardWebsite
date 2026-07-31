using MediatR;

namespace Services.AuthService.Application.Application.Command
{
    public record ResetPasswordCommand(Guid UserId, string ResetToken, string NewPassword) : IRequest<bool>;
}