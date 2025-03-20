using API.Mappers.Journal.PersonalizedAccountingCards;
using Contracts.Journal.PersonalizedAccountingCards.StudentEncouragements;
using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/personalizedaccountingcard/studentencouragement")]
    public class StudentEncouragementsController : ControllerBase
    {
        private readonly IStudentEncouragementsRepository _studentEncouragementsRepository;

        public StudentEncouragementsController(IStudentEncouragementsRepository studentEncouragementsRepository)
        {
            _studentEncouragementsRepository = studentEncouragementsRepository;
        }

        [HttpGet("{cardId}")]
        public async Task<ActionResult<List<StudentEncouragementResponse>>> GetEncouragementsByCard([FromRoute] int cardId)
        {
            var records = await _studentEncouragementsRepository.GetByCardIdAsync(cardId);
            if (records == null) return BadRequest();

            var response = records.Select(p => p.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<StudentEncouragementResponse>> AddStudentEncouragement([FromBody] CreateStudentEncouragementRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var created = await _studentEncouragementsRepository.CreateAsync(record);
            if (created == null) return BadRequest();

            var response = created.ToResponse();
            return CreatedAtAction(nameof(AddStudentEncouragement), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StudentEncouragementResponse>> UpdateStudentEncouragement([FromRoute] int id,
            [FromBody] UpdateStudentEncouragementRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();
            var updated = await _studentEncouragementsRepository.UpdateAsync(id, record);
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _studentEncouragementsRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
