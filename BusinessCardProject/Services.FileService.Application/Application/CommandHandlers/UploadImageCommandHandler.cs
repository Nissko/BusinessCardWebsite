using MediatR;
using Services.FileService.Application.Application.Command;
using Services.FileService.Application.Common.Interfaces;

namespace Services.FileService.Application.Application.CommandHandlers
{
    public class UploadImageCommandHandler(IImageRepository _repository) : IRequestHandler<UploadImageCommand, string>
    {
        public async Task<string> Handle(UploadImageCommand request, CancellationToken ct)
        {
            return await _repository.SaveImage(request.FileStream, request.FileName, request.ContentType, ct);
        }
    }
}