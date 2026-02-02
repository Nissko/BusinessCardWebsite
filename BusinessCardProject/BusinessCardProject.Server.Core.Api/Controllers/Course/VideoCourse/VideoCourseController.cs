using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using ContractualDtos.DTO.Course.VideoCourse.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardProject.Server.Core.Api.Controllers.Course.VideoCourse;

[ApiController]
[Route("api/[controller]")]
public class VideoCourseController : ControllerBase
{
    private readonly IVideoCourseRepository _repository;

    public VideoCourseController(IVideoCourseRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return result.Count != 0 ? Ok(result) : NoContent();
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateVideoCourseRequestDto dto)
    {
        var result = await _repository.Create(dto);
        return result != null ? Created() : BadRequest();
    }

    [HttpGet("find/{id}")]
    public async Task<IActionResult> FindAsync(Guid id)
    {
        var result = await _repository.Read(id);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateVideoCourseRequestDto dto)
    {
        var result = await _repository.Update(dto);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _repository.Delete(id);
        return result ? Ok() : NoContent();
    }
}