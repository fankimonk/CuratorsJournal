using API.Mappers;
using Contracts.Journal.StudentHealthCards;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/healthcard")]
    public class StudentHealthCardController(IStudentHealthCardRepository studentHealthCardRepository) : ControllerBase
    {
        private readonly IStudentHealthCardRepository _studentHealthCardRepository = studentHealthCardRepository;

        [HttpGet("{pageId}")]
        public async Task<ActionResult<HealthCardPageResponse>> GetRecordsByPage([FromRoute] int pageId)
        {
            var records = await _studentHealthCardRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var response = records.Select(r => r.ToResponse()).ToList();
            return Ok(new HealthCardPageResponse(pageId, response));
        }

        [HttpPost]
        public async Task<ActionResult<HealthCardRecordResponse>> AddRecord([FromBody] CreateHealthCardRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var createdRecord = await _studentHealthCardRepository.CreateAsync(record);
            if (createdRecord == null) return BadRequest();

            var response = createdRecord.ToResponse();
            return CreatedAtAction(nameof(AddRecord), response);
        }

        [HttpPut("{recordId}")]
        public async Task<ActionResult<HealthCardRecordResponse>> UpdateRecord([FromRoute] int recordId, [FromBody] UpdateHealthCardRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var updatedRecord = await _studentHealthCardRepository.UpdateAsync(recordId, record);
            if (updatedRecord == null) return BadRequest();

            var response = updatedRecord.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            if (!await _studentHealthCardRepository.DeleteAsync(recordId)) return NotFound();

            return NoContent();
        }
    }
}
