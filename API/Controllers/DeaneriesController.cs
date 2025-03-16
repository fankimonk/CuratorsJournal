using API.Mappers;
using Contracts.Deaneries;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/deanery")]
    [ApiController]
    public class DeaneriesController : ControllerBase
    {
        private readonly IDeaneriesRepository _deaneriesRepository;

        public DeaneriesController(IDeaneriesRepository deaneriesRepository)
        {
            _deaneriesRepository = deaneriesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<DeaneryResponse>>> GetAll()
        {
            var positions = await _deaneriesRepository.GetAllAsync();

            var response = positions.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<DeaneryResponse>> Create([FromBody] CreateDeaneryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _deaneriesRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DeaneryResponse>> Update([FromRoute] int id, [FromBody] UpdateDeaneryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _deaneriesRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _deaneriesRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
