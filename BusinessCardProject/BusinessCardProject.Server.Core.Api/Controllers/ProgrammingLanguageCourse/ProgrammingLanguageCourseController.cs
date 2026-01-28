using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using ContractualDtos.DTO.ProgrammingLanguageCourse.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardProject.Server.Core.Api.Controllers.ProgrammingLanguageCourse;

[ApiController]
[Route("api/[controller]")]
public class ProgrammingLanguageCourseController : ControllerBase
{
    private readonly IProgrammingLanguageRepository _repository;

    public ProgrammingLanguageCourseController(IProgrammingLanguageRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    [HttpGet("get")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return result.Count == 0 ? NotFound(result) : Ok(result);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateProgrammingLanguageRequestDto dto)
    {
        var result = await _repository.Create(dto);
        return Ok(result);
    }
    
    
    [HttpGet("find/{id}")]
    public async Task<IActionResult> Find(Guid id)
    {
        var result = await _repository.Read(id);
        return result != null ? Ok(result) : NotFound(result);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> Update([FromBody] UpdateProgrammingLanguageRequestDto dto)
    {
        var result = await _repository.Update(dto);
        return result != null ? Ok(result) : NotFound(result);
    }

    [HttpPatch("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _repository.Delete(id);
        return result ? Ok() : NotFound(result);
    }
}