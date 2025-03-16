using API.Mappers;
using Contracts.Positions;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/position")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionsRepository _positionsRepository;

        public PositionsController(IPositionsRepository positionsRepository)
        {
            _positionsRepository = positionsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PositionResponse>>> GetAll()
        {
            var positions = await _positionsRepository.GetAllAsync();

            var response = positions.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<PositionResponse>> Create([FromBody] CreatePositionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _positionsRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PositionResponse>> Update([FromRoute] int id, [FromBody] UpdatePositionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _positionsRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _positionsRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
