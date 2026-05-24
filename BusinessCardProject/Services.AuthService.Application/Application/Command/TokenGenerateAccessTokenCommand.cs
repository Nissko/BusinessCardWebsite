using Dtos.DTO.User;
using MediatR;

namespace Services.AuthService.Application.Application.Command
{
    public class TokenGenerateAccessTokenCommand(UserDto user) : IRequest<string>
    {
        public UserDto User { get; private set; } = user;
    }
}