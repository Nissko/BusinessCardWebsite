using BusinessCardProject.Server.Core.Application.Application.Command;
using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using ContractualDtos.DTO.DynamicUpdateEntities;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardProject.Server.Core.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoreController : ControllerBase
{
    private readonly ICustomMediator _mediator;
    
    public CoreController(ICustomMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    [HttpPut("dynamic-update")]
    public async Task<IActionResult> UpdateAsync([FromBody] DynamicClassDto dto)
    {
        var result = await _mediator.Send(new DynamicUpdateCommand(dto.Id, dto.ClassApiName, dto.FieldApiName, dto.Value));
        return result ? Ok(result) : BadRequest(false);
    }
}