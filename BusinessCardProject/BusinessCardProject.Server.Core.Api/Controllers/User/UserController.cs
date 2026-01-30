using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.User;
using ContractualDtos.DTO.User.UserProfile.Dtos;
using ContractualDtos.DTO.User.UserProfile.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardProject.Server.Core.Api.Controllers.User;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
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
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserProfileRequestDto dto)
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
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserProfileRequestDto dto)
    {
        var result = await _repository.Update(dto);
        return result != null ? Ok(result) : NoContent();
    }

    /// <summary>
    /// TODO: Добавить CurrentUserService чтобы смотреть на пользователя, который посылает этот запрос
    /// </summary>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> SafeDeleteAsync(Guid id)
    {
        var result = await _repository.SafeDelete(id);
        return result ? Ok() : NoContent();
    }
    
    /// <summary>
    /// TODO: Добавить CurrentUserService чтобы смотреть на пользователя, который посылает этот запрос
    /// </summary>
    [HttpPost("safe-recovery/{id}")]
    public async Task<IActionResult> RecoveryUserProfileAsync(Guid id)
    {
        var result = await _repository.RecoveryUserProfile(id);
        return result ? Ok() : NoContent();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] AuthUserDto dto)
    {
        var result = await _repository.Login(dto);
        return result ? Ok(result) : BadRequest("Некорректно введены данные от учетной записи пользователя");
    }
}