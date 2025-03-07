using API.Mappers;
using API.Mappers.Journal;
using Contracts.Journal.CuratorsParticipationInPedagogicalSeminars;
using Contracts.Journal.EducationalProcessSchedule;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/participationinpedagogicalseminars")]
    public class ParticipationInPedagogicalSeminarsController(
        IParticipationInPedagogicalSeminarsRepository participationInPedagogicalSeminarsRepository) : ControllerBase
    {
        private readonly IParticipationInPedagogicalSeminarsRepository _participationInPedagogicalSeminarsRepository
            = participationInPedagogicalSeminarsRepository;

        [HttpGet("{pageId}")]
        public async Task<ActionResult<ParticipationInPedagogicalSeminarsPageResponse>> GetRecordsByPage([FromRoute] int pageId)
        {
            var records = await _participationInPedagogicalSeminarsRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var response = records.Select(r => r.ToResponse()).ToList();
            return Ok(new ParticipationInPedagogicalSeminarsPageResponse(pageId, response));
        }

        [HttpPost]
        public async Task<ActionResult<ParticipationInPedagogicalSeminarsRecordResponse>> AddRecord(
            [FromBody] CreateParticipationInPedagogicalSeminarsRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var createdRecord = await _participationInPedagogicalSeminarsRepository.CreateAsync(record);
            if (createdRecord == null) return BadRequest();

            var response = createdRecord.ToResponse();
            return CreatedAtAction(nameof(AddRecord), response);
        }

        [HttpPut("{recordId}")]
        public async Task<ActionResult<ParticipationInPedagogicalSeminarsRecordResponse>> UpdateRecord([FromRoute] int recordId,
            [FromBody] UpdateParticipationInPedagogicalSeminarsRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var updatedRecord = await _participationInPedagogicalSeminarsRepository.UpdateAsync(recordId, record);
            if (updatedRecord == null) return BadRequest();

            var response = updatedRecord.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            if (!await _participationInPedagogicalSeminarsRepository.DeleteAsync(recordId)) return NotFound();

            return NoContent();
        }
    }
}
