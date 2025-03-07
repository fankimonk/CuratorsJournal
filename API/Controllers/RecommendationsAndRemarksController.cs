using API.Mappers;
using API.Mappers.Journal;
using Contracts.Journal.RecommendationsAndRemarks;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/recommendationsandremarks")]
    public class RecommendationsAndRemarksController : ControllerBase
    {
        private readonly IRecommendationsAndRemarksRepository _entityRepository;

        public RecommendationsAndRemarksController(IRecommendationsAndRemarksRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        [HttpGet("{pageId}")]
        public async Task<ActionResult<RecommendationsAndRemarksPageResponse>> GetByPage([FromRoute] int pageId)
        {
            var records = await _entityRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var response = records.Select(p => p.ToResponse()).ToList();
            return Ok(new RecommendationsAndRemarksPageResponse(pageId, response));
        }

        [HttpPost]
        public async Task<ActionResult<RecommendationsAndRemarksRecordResponse>> AddRecord(
            [FromBody] CreateRecommendationsAndRemarksRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var createdRecord = await _entityRepository.CreateAsync(record);
            if (createdRecord == null) return BadRequest();

            var response = createdRecord.ToResponse();
            return CreatedAtAction(nameof(AddRecord), response);
        }

        [HttpPut("{recordId}")]
        public async Task<ActionResult<RecommendationsAndRemarksRecordResponse>> UpdateContactPhone([FromRoute] int recordId,
            [FromBody] UpdateRecommendationsAndRemarksRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();
            var updatedRecord = await _entityRepository.UpdateAsync(recordId, record);
            if (updatedRecord == null) return BadRequest();

            var response = updatedRecord.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteContactPhone([FromRoute] int recordId)
        {
            if (!await _entityRepository.DeleteAsync(recordId)) return NotFound();

            return NoContent();
        }
    }
}
