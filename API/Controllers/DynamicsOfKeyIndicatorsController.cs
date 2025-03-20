using API.Mappers;
using API.Mappers.Journal;
using Contracts.Journal.DynamicsOfKeyIndicators;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/dynamicsofkeyindicators")]
    public class DynamicsOfKeyIndicatorsController : ControllerBase
    {
        private readonly IDynamicsOfKeyIndicatorsRepository _entityRepository;

        public DynamicsOfKeyIndicatorsController(IDynamicsOfKeyIndicatorsRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        [HttpGet("{pageId}")]
        public async Task<ActionResult<DynamicsOfKeyIndicatorsPageResponse>> GetByPage([FromRoute] int pageId)
        {
            var records = await _entityRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var response = records.Select(p => p.ToResponse()).ToList();
            return Ok(new DynamicsOfKeyIndicatorsPageResponse(pageId, response));
        }

        //[HttpPost]
        //public async Task<ActionResult<KeyIndicatorByCourseResponse>> AddValue([FromBody] CreateKeyIndicatorValueRequest request)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    var value = request.ToEntity();

        //    var createdValue = await _entityRepository.AddValueAsync(value);
        //    if (createdValue == null) return BadRequest();

        //    var response = createdValue.ToResponse();
        //    return CreatedAtAction(nameof(AddValue), response);
        //}

        [HttpPut("update/{recordId}")]
        public async Task<ActionResult<DynamicsOfKeyIndicatorsRecordResponse>> Update([FromRoute] int recordId,
            [FromBody] UpdateDynamicsOfKeyIndicatorsRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();
            var updatedRecord = await _entityRepository.UpdateAsync(recordId, record);
            if (updatedRecord == null) return BadRequest();

            var response = updatedRecord.ToResponse();
            return Ok(response);
        }

        [HttpPut("updatevalue/{valueId}")]
        public async Task<ActionResult<KeyIndicatorByCourseResponse>> UpdateValue([FromRoute] int valueId,
            [FromBody] UpdateKeyIndicatorValueRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();
            var updatedRecord = await _entityRepository.UpdateValueAsync(valueId, record);
            if (updatedRecord == null) return BadRequest();

            var response = updatedRecord.ToResponse();
            return Ok(response);
        }

        //[HttpDelete("/delete/{recordId}")]
        //public async Task<ActionResult> Delete([FromRoute] int recordId)
        //{
        //    if (!await _entityRepository.DeleteAsync(recordId)) return NotFound();

        //    return NoContent();
        //}

        [HttpPost("addcourse/{pageId}")]
        public async Task<ActionResult<DynamicsOfKeyIndicatorsPageResponse>> AddCourse([FromRoute] int pageId)
        {
            var updated = await _entityRepository.AddCourseAsync(pageId);
            if (updated == null) return BadRequest();

            var response = updated.Select(p => p.ToResponse()).ToList();
            return Ok(new DynamicsOfKeyIndicatorsPageResponse(pageId, response));
        }

        [HttpDelete("deletecourse/{pageId}")]
        public async Task<ActionResult<DynamicsOfKeyIndicatorsPageResponse>> DeleteCourse([FromRoute] int pageId)
        {
            var updated = await _entityRepository.DeleteCourseAsync(pageId);
            if (updated == null) return BadRequest();

            var response = updated.Select(p => p.ToResponse()).ToList();
            return Ok(new DynamicsOfKeyIndicatorsPageResponse(pageId, response));
        }
    }
}
