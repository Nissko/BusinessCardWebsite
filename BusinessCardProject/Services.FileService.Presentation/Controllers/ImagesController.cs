using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.FileService.Application.Application.Command;

namespace Services.FileService.Presentation.Controllers
{
    [ApiController]
    [Route("FilesServiceGrpcService")]
    public class ImagesController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetImage(string fileId, CancellationToken ct)
        {
            if (fileId.Contains("..") || fileId.Contains('/') || fileId.Contains('\\'))
            {
                return BadRequest("Недопустимый идентификатор файла");
            }
            
            var (stream, contentType) = await mediator.Send(new GetImageQuery(fileId), ct);
            return File(stream, contentType);
        }
    }
}