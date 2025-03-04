using API.Mappers;
using Contracts.Journal.LiteratureWork;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/literaturework")]
    public class LiteratureWorkController : ControllerBase
    {
        private readonly ILiteratureWorkRepository _literatureWorkRepository;

        public LiteratureWorkController(ILiteratureWorkRepository literatureWorkRepository)
        {
            _literatureWorkRepository = literatureWorkRepository;
        }

        [HttpGet("{pageId}")]
        public async Task<ActionResult<LiteratureWorkPageResponse>> GetByPage([FromRoute] int pageId)
        {
            var records = await _literatureWorkRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var response = records.Select(p => p.ToResponse()).ToList();
            return Ok(new LiteratureWorkPageResponse(pageId, response));
        }

        [HttpPost]
        public async Task<ActionResult<LiteratureWorkRecordResponse>> AddRecord([FromBody] CreateLiteratureWorkRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var createdRecord = await _literatureWorkRepository.CreateAsync(record);
            if (createdRecord == null) return BadRequest();

            var response = createdRecord.ToResponse();
            return CreatedAtAction(nameof(AddRecord), response);
        }

        [HttpPut("{recordId}")]
        public async Task<ActionResult<LiteratureWorkRecordResponse>> UpdateRecord([FromRoute] int recordId, 
            [FromBody] UpdateLiteratureWorkRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();
            var updatedRecord = await _literatureWorkRepository.UpdateAsync(recordId, record);
            if (updatedRecord == null) return BadRequest();

            var response = updatedRecord.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            if (!await _literatureWorkRepository.DeleteAsync(recordId)) return NotFound();

            return NoContent();
        }
    }
}
