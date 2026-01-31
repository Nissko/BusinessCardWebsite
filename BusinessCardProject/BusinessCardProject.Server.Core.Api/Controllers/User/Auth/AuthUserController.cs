using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.User;
using ContractualDtos.DTO.User.UserProfile.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardProject.Server.Core.Api.Controllers.User.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthUserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public AuthUserController(IUserRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// TODO: Сделать возврат AccessToken(-a)
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] AuthUserDto dto)
    {
        var result = await _repository.Login(dto);
        return result ? Ok(result) : BadRequest("Некорректно введены данные от учетной записи пользователя");
    }
}