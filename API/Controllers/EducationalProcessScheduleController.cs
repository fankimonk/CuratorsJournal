using API.Mappers;
using Contracts.Journal.EducationalProcessSchedule;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/educationalprocessschedule")]
    public class EducationalProcessScheduleController(IEducationalProcessScheduleRepository educationalProcessScheduleRepository) : ControllerBase
    {
        private readonly IEducationalProcessScheduleRepository _educationalProcessScheduleRepository = educationalProcessScheduleRepository;

        [HttpGet("{pageId}")]
        public async Task<ActionResult<EducationalProcessSchedulePageResponse>> GetRecordsByPage([FromRoute] int pageId)
        {
            var records = await _educationalProcessScheduleRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var response = records.Select(r => r.ToResponse()).ToList();
            return Ok(new EducationalProcessSchedulePageResponse(pageId, response));
        }

        [HttpPost]
        public async Task<ActionResult<EducationalProcessScheduleRecordResponse>> AddRecord([FromBody] CreateEducationalProcessScheduleRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var createdRecord = await _educationalProcessScheduleRepository.CreateAsync(record);
            if (createdRecord == null) return BadRequest();

            var response = createdRecord.ToResponse();
            return CreatedAtAction(nameof(AddRecord), response);
        }

        [HttpPut("{recordId}")]
        public async Task<ActionResult<EducationalProcessScheduleRecordResponse>> UpdateRecord([FromRoute] int recordId,
            [FromBody] UpdateEducationalProcessScheduleRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var updatedRecord = await _educationalProcessScheduleRepository.UpdateAsync(recordId, record);
            if (updatedRecord == null) return BadRequest();

            var response = updatedRecord.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            if (!await _educationalProcessScheduleRepository.DeleteAsync(recordId)) return NotFound();

            return NoContent();
        }
    }
}
