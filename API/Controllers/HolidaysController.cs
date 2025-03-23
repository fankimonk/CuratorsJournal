using API.Mappers.Journal;
using Contracts.Journal.Holidays;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/holiday")]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidaysRepository _holidaysRepository;

        public HolidaysController(IHolidaysRepository holidaysRepository)
        {
            _holidaysRepository = holidaysRepository;
        }

        [HttpGet("type")]
        public async Task<ActionResult<List<HolidayTypeResponse>>> GetAllTypes()
        {
            var types = await _holidaysRepository.GetAllTypesAsync();
            var response = types.Select(t => t.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpGet("{pageId}")]
        public async Task<ActionResult<HolidaysPageResponse>> GetByPageId([FromRoute] int pageId)
        {
            var holidayTypes = await _holidaysRepository.GetByPageIdAsync(pageId);
            if (holidayTypes == null) return BadRequest();

            var holidayTypeResponses = holidayTypes.Select(ht => ht.ToResponse()).ToList();

            return Ok(new HolidaysPageResponse(pageId, holidayTypeResponses));
        }

        [HttpPost]
        public async Task<ActionResult<HolidayResponse>> Create([FromBody] CreateHolidayRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var holiday = request.ToEntity();

            var created = await _holidaysRepository.CreateAsync(holiday);
            if (created == null) return BadRequest();

            return CreatedAtAction(nameof(Create), created.ToResponse());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<HolidayResponse>> Update([FromRoute] int id, [FromBody] UpdateHolidayRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var holiday = request.ToEntity();

            var updated = await _holidaysRepository.UpdateAsync(id, holiday);
            if (updated == null) return BadRequest();

            return Ok(updated.ToResponse());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _holidaysRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }

        [HttpPost("type")]
        public async Task<ActionResult<HolidayTypeResponse>> CreateType([FromBody] CreateHolidayTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var holiday = request.ToEntity();

            var created = await _holidaysRepository.CreateTypeAsync(holiday);
            if (created == null) return BadRequest();

            return CreatedAtAction(nameof(CreateType), created.ToResponse());
        }

        [HttpPut("type/{id}")]
        public async Task<ActionResult<HolidayTypeResponse>> UpdateType([FromRoute] int id, [FromBody] UpdateHolidayTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var holiday = request.ToEntity();

            var updated = await _holidaysRepository.UpdateTypeAsync(id, holiday);
            if (updated == null) return BadRequest();

            return Ok(updated.ToResponse());
        }

        [HttpDelete("type/{id}")]
        public async Task<ActionResult> DeleteType([FromRoute] int id)
        {
            if (!await _holidaysRepository.DeleteTypeAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
