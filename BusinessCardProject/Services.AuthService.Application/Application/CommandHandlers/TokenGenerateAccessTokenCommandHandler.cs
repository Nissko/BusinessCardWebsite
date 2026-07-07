using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Services.AuthService.Application.Application.Command;
using Services.AuthService.Application.Common.Interfaces;

namespace Services.AuthService.Application.Application.CommandHandlers
{
    public class TokenGenerateAccessTokenCommandHandler : IRequestHandler<TokenGenerateAccessTokenCommand, string>
    {
        private readonly RSA _signingKey;
        private TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(15);
        private readonly IUserRepository _userRepository;
    
        public TokenGenerateAccessTokenCommandHandler(RSA signingKey, IUserRepository userRepository)
        {
            _signingKey = signingKey ?? throw new ArgumentNullException(nameof(signingKey));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<string> Handle(TokenGenerateAccessTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, request.User.Id.ToString()),
                    new(JwtRegisteredClaimNames.Name, request.User.NickName),
                    new(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.Add(AccessTokenLifetime).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };

                var userRoles = await _userRepository.GetRoles(request.User.Id);
                claims.AddRange(userRoles.Select(userRole => new Claim("role", userRole)));

                var token = new JwtSecurityToken(
                    issuer: "https://bytecode.splinterkeenetic.netcraze.club/auth",
                    audience: "grpc-services",
                    claims: claims,
                    expires: DateTime.UtcNow.Add(AccessTokenLifetime),
                    signingCredentials: new SigningCredentials(new RsaSecurityKey(_signingKey), SecurityAlgorithms.RsaSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}