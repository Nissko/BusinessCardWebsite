using MediatR;
using Services.AuthService.Application.Application.Command;

namespace Services.AuthService.Application.Application.CommandHandlers
{
    public class GetUserAgentCommandHandler : IRequestHandler<GetUserAgentCommand, string?>
    {
        public Task<string?> Handle(GetUserAgentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userAgent = request.Context.RequestHeaders
                    .FirstOrDefault(x => x.Key.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
                    ?.Value;

                return Task.FromResult(string.IsNullOrEmpty(userAgent) 
                    ? null 
                    : userAgent.Length > 512 ? userAgent[..512] : userAgent);
            }
            catch (Exception exception)
            {
                return Task.FromException<string?>(exception);
            }
        }
    }
}