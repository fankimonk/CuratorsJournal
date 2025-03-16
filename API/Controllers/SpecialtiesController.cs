using API.Mappers;
using Contracts.Specialties;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/specialty")]
    [ApiController]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtiesRepository _specialtiesRepository;

        public SpecialtiesController(ISpecialtiesRepository specialtiesRepository)
        {
            _specialtiesRepository = specialtiesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<SpecialtyResponse>>> GetAll()
        {
            var positions = await _specialtiesRepository.GetAllAsync();

            var response = positions.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<SpecialtyResponse>> Create([FromBody] CreateSpecialtyRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _specialtiesRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SpecialtyResponse>> Update([FromRoute] int id, [FromBody] UpdateSpecialtyRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _specialtiesRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _specialtiesRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
