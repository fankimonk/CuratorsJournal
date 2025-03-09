using API.Mappers;
using API.Mappers.Journal;
using Contracts.Journal.StudentHealthCards;
using DataAccess.Interfaces;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/healthcard")]
    public class StudentHealthCardController(IStudentHealthCardRepository studentHealthCardRepository,
        IHealthCardAttributesRepository attributesRepository) : ControllerBase
    {
        private readonly IStudentHealthCardRepository _studentHealthCardRepository = studentHealthCardRepository;
        private readonly IHealthCardAttributesRepository _attributesRepository = attributesRepository;

        [HttpGet("{pageId}")]
        public async Task<ActionResult<HealthCardPageResponse>> GetRecordsByPage([FromRoute] int pageId)
        {
            var records = await _studentHealthCardRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var attributes = await _attributesRepository.GetByPageId(pageId);
            if (attributes == null) return BadRequest();

            var response = records.Select(r => r.ToResponse()).ToList();
            return Ok(new HealthCardPageResponse(pageId, response, attributes.ToResponse()));
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

        [HttpPut("updateattributes/{attributesId}")]
        public async Task<ActionResult<HealthCardPageAttributesResponse>> UpdateAttributes([FromRoute] int attributesId,
            [FromBody] UpdateHealthCardPageAttributesRequest request)
        {
            var attributes = await _attributesRepository.UpdateAcademicYear(attributesId, request.AcademicYearId);
            if (attributes == null) return BadRequest();

            var response = attributes.ToResponse();
            return Ok(response);
        }
    }
}
