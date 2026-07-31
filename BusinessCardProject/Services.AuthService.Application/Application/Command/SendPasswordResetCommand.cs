using MediatR;

namespace Services.AuthService.Application.Application.Command
{
    public record SendPasswordResetCommand(string Email) : IRequest<bool>;
}