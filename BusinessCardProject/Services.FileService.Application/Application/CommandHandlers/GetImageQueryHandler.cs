using MediatR;
using Services.FileService.Application.Application.Command;
using Services.FileService.Application.Common.Interfaces;

namespace Services.FileService.Application.Application.CommandHandlers
{
    public class GetImageQueryHandler(IImageRepository repository) : IRequestHandler<GetImageQuery, (Stream, string)>
    {
        public async Task<(Stream, string)> Handle(GetImageQuery request, CancellationToken ct)
        {
            return await repository.GetAsync(request.FileId, ct);
        }
    }
}