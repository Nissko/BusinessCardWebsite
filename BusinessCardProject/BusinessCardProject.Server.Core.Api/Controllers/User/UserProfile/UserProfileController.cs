using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.User;
using ContractualDtos.DTO.Table;
using ContractualDtos.DTO.User.UserProfile.Dtos;
using ContractualDtos.DTO.User.UserProfile.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardProject.Server.Core.Api.Controllers.User.UserProfile
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserProfileController(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    
        /// <summary>
        /// TODO: Добавить CurrentUserService чтобы смотреть на пользователя, который посылает этот запрос
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<TableResponse<UserProfileDtos>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "Ascending")
        {
            var result = await _repository.GetAllAsync(page, pageSize, search, sortBy, sortDirection);
            return result.TotalCount != 0 ? Ok(result) : NoContent();
        }

        /// <summary>
        /// TODO: Добавить CurrentUserService чтобы смотреть на пользователя, который посылает этот запрос
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserProfileRequestDto dto)
        {
            var result = await _repository.Create(dto);
            return result != null ? Created() : BadRequest();
        }
    
        /// <summary>
        /// TODO: Добавить CurrentUserService чтобы смотреть на пользователя, который посылает этот запрос
        /// </summary>
        [HttpGet("find/{id}")]
        public async Task<IActionResult> FindAsync(Guid id)
        {
            var result = await _repository.Read(id);
            return result != null ? Ok(result) : NoContent();
        }

        /// <summary>
        /// TODO: Добавить CurrentUserService чтобы смотреть на пользователя, который посылает этот запрос
        /// </summary>
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

        [HttpPost("update-user-settings")]
        public async Task<IActionResult> UpdateUserSettingsAsync([FromBody] UserProfileSettingsDto settings)
        {
            var result = await _repository.UpdateUserSettings(settings);
            return result ? Ok() : NoContent();
        }
    
        /*[HttpPost("add-role/{id}")]
        public async Task<IActionResult> AddNewRoleAsync(AddNewUserRoleRequestDto dto)
        {
            var result = await _repository.AddNewRole(dto);
            return result ? Ok() : NoContent();
        }*/
    }
}