using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardProject.Server.Core.Api.Controllers.Course.VideoCourse;

[ApiController]
[Route("api/[controller]")]
public class VideoCourseController(ICustomMediator mediator) : ControllerBase
{
    private readonly ICustomMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    /*[HttpGet("get-user-id")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetSubjects()
    {
        var result = await _mediator.Send(new GetUsersQuery(Guid.NewGuid()));
        
        return Ok(result);
    }
    
    [HttpGet("get-random-guid")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetRandomGuid()
    {
        var result = await _mediator.Send(new GetGuidQuery());
        
        return Ok(result);
    }*/
}