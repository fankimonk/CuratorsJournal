using API.Mappers;
using Contracts.PEGroups;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/pegroup")]
    [ApiController]
    public class PEGroupsController : ControllerBase
    {
        private readonly IPEGroupsRepository _peGroupsRepository;

        public PEGroupsController(IPEGroupsRepository peGroupsRepository)
        {
            _peGroupsRepository = peGroupsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PEGroupResponse>>> GetAll()
        {
            var positions = await _peGroupsRepository.GetAllAsync();

            var response = positions.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<PEGroupResponse>> Create([FromBody] CreatePEGroupRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _peGroupsRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PEGroupResponse>> Update([FromRoute] int id, [FromBody] UpdatePEGroupRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _peGroupsRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _peGroupsRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
