using System.Security.Cryptography;
using MediatR;
using Services.AuthService.Application.Application.Command;

namespace Services.AuthService.Application.Application.CommandHandlers
{
    public class TokenGenerateRefreshTokenCommandHandler : IRequestHandler<TokenGenerateRefreshTokenCommand, string>
    {
        public Task<string> Handle(TokenGenerateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var randomBytes = new byte[64];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomBytes);
                return Task.FromResult(Convert.ToBase64String(randomBytes));
            }
            catch (Exception exception)
            {
                return Task.FromException<string>(exception);
            }
        }
    }
}