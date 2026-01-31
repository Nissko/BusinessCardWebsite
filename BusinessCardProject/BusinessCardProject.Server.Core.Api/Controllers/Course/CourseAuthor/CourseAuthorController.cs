using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.User;
using ContractualDtos.DTO.Course.CourseAuthor.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardProject.Server.Core.Api.Controllers.Course.CourseAuthor;

[ApiController]
[Route("api/[controller]")]
public class CourseAuthorController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ICourseAuthorRepository _courseAuthorRepository;

    public CourseAuthorController(IUserRepository userRepository, ICourseAuthorRepository courseAuthorRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _courseAuthorRepository = courseAuthorRepository
                                  ?? throw new ArgumentNullException(nameof(courseAuthorRepository));
    }
    
    [HttpGet("get")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _courseAuthorRepository.GetAllAsync();
        return result.Count != 0 ? Ok(result) : NoContent();
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] Guid id)
    {
        var user = await _userRepository.Read(id);
        if (user == null) return BadRequest();
        var result = await _courseAuthorRepository.Create(new CreateCourseAuthorRequestDto(
            user.Surname, user.Name, user.Patronymic, user.AltName));
        return result != null ? Created() : BadRequest();
    }
    
    [HttpGet("find/{id}")]
    public async Task<IActionResult> FindAsync(Guid id)
    {
        var result = await _courseAuthorRepository.Read(id);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] Guid userId, Guid authorId)
    {
        var user = await _userRepository.Read(userId);
        if (user == null) return BadRequest();
        var result = await _courseAuthorRepository.Update(new UpdateCourseAuthorRequestDto(
            authorId, user.Surname, user.Name, user.Patronymic, user.AltName));
        return result != null ? Ok(result) : NoContent();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _courseAuthorRepository.Delete(id);
        return result ? Ok() : NoContent();
    }
}