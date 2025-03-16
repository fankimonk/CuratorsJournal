using API.Mappers;
using Contracts.Subjects;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectsRepository _subjectsRepository;

        public SubjectsController(ISubjectsRepository subjectsRepository)
        {
            _subjectsRepository = subjectsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubjectResponse>>> GetAll()
        {
            var positions = await _subjectsRepository.GetAllAsync();

            var response = positions.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<SubjectResponse>> Create([FromBody] CreateSubjectRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _subjectsRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubjectResponse>> Update([FromRoute] int id, [FromBody] UpdateSubjectRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _subjectsRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _subjectsRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
