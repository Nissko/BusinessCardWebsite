using Grpc.Core;
using MediatR;

namespace Services.AuthService.Application.Application.Command
{
    public class GetUserAgentCommand : IRequest<string?>
    {
        public GetUserAgentCommand(ServerCallContext context)
        {
            Context = context;
        }

        public ServerCallContext Context { get; private set; }
    }
}