using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.FileService.Application.Application.Command;

namespace Services.FileService.Presentation.Controllers
{
    public class ImagesController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetImage(string fileId, CancellationToken ct)
        {
            var (stream, contentType) = await mediator.Send(new GetImageQuery(fileId), ct);
            return File(stream, contentType);
        }
    }
}