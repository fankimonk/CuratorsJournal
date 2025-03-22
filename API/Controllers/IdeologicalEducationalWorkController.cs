using API.Mappers;
using API.Mappers.Journal;
using Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting;
using DataAccess.Interfaces;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/ideologicalandeducationalwork")]
    public class IdeologicalEducationalWorkController(IIdeologicalEducationalWorkRepository ideologicalEducationalWorkRepository,
        IIdeologicalAndEducationalWorkPageAttributesRepository attributesRepository) : ControllerBase
    {
        private readonly IIdeologicalEducationalWorkRepository _ideologicalEducationalWorkRepository = ideologicalEducationalWorkRepository;
        private readonly IIdeologicalAndEducationalWorkPageAttributesRepository _attributesRepository = attributesRepository;

        [HttpGet("{pageId}")]
        public async Task<ActionResult<IdeologicalEducationalWorkPageResponse>> GetRecordsByPage([FromRoute] int pageId)
        {
            var records = await _ideologicalEducationalWorkRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var response = records.Select(r => r.ToResponse()).ToList();

            var attributes = await _attributesRepository.GetByPageId(pageId);
            if (attributes == null) return BadRequest();

            return Ok(new IdeologicalEducationalWorkPageResponse(pageId, attributes.ToResponse(), response));
        }

        [HttpPost]
        public async Task<ActionResult<IdeologicalEducationalWorkRecordResponse>> AddRecord(
            [FromBody] CreateIdeologicalEducationalWorkRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var createdRecord = await _ideologicalEducationalWorkRepository.CreateAsync(record);
            if (createdRecord == null) return BadRequest();

            var response = createdRecord.ToResponse();
            return CreatedAtAction(nameof(AddRecord), response);
        }

        [HttpPut("{recordId}")]
        public async Task<ActionResult<IdeologicalEducationalWorkRecordResponse>> UpdateRecord([FromRoute] int recordId,
            [FromBody] UpdateIdeologicalEducationalWorkRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var updatedRecord = await _ideologicalEducationalWorkRepository.UpdateAsync(recordId, record);
            if (updatedRecord == null) return BadRequest();

            var response = updatedRecord.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            if (!await _ideologicalEducationalWorkRepository.DeleteAsync(recordId)) return NotFound();

            return NoContent();
        }

        [HttpPut("updateattributes/{attributesId}")]
        public async Task<ActionResult<IdeologicalAndEducationalWorkAttributesResponse>> UpdateAttributes([FromRoute] int attributesId,
            [FromBody] UpdateIdeologicalAndEducationalWorkAttributesRequest request)
        {
            var attributes = await _attributesRepository.UpdateAttributes(attributesId, request.Month, request.Year);
            if (attributes == null) return BadRequest();

            var response = attributes.ToResponse();
            return Ok(response);
        }
    }
}
