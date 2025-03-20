using API.Mappers.Journal.PersonalizedAccountingCards;
using Contracts.Journal.PersonalizedAccountingCards.StudentDisciplinaryResponsibilities;
using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/personalizedaccountingcard/studentdisciplinaryresponsibility")]
    public class StudentDisciplinaryResponsibilitiesController : ControllerBase
    {
        private readonly IStudentDisciplinaryResponsibilitiesRepository _entityRepository;

        public StudentDisciplinaryResponsibilitiesController(IStudentDisciplinaryResponsibilitiesRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        [HttpGet("{cardId}")]
        public async Task<ActionResult<List<StudentDisciplinaryResponsibilityResponse>>> GetByCard([FromRoute] int cardId)
        {
            var records = await _entityRepository.GetByCardIdAsync(cardId);
            if (records == null) return BadRequest();

            var response = records.Select(p => p.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<StudentDisciplinaryResponsibilityResponse>> Add([FromBody] CreateStudentDisciplinaryResponsibilityRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var created = await _entityRepository.CreateAsync(record);
            if (created == null) return BadRequest();

            var response = created.ToResponse();
            return CreatedAtAction(nameof(Add), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StudentDisciplinaryResponsibilityResponse>> Update([FromRoute] int id,
            [FromBody] UpdateStudentStudentDisciplinaryResponsibilityRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();
            var updated = await _entityRepository.UpdateAsync(id, record);
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _entityRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
