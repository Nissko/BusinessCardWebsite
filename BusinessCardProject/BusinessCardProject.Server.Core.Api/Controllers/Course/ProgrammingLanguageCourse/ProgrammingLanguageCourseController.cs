using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardProject.Server.Core.Api.Controllers.Course.ProgrammingLanguageCourse
{
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
            return result.Count != 0 ? Ok(result) : NoContent();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateProgrammingLanguageRequestDto dto)
        {
            var result = await _repository.Create(dto);
            return result != null ? Created() : BadRequest(result);
        }


        [HttpGet("find/{id}")]
        public async Task<IActionResult> FindAsync(Guid id)
        {
            var result = await _repository.Read(id);
            return result != null ? Ok(result) : NoContent();
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProgrammingLanguageRequestDto dto)
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
}