using API.Mappers;
using Contracts.ChronicDiseases;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/chronicdisease")]
    [ApiController]
    public class ChronicDiseasesController : ControllerBase
    {
        private readonly IChronicDiseasesRepository _chronicDiseasesRepository;

        public ChronicDiseasesController(IChronicDiseasesRepository chronicDiseasesRepository)
        {
            _chronicDiseasesRepository = chronicDiseasesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChronicDiseaseResponse>>> GetAll()
        {
            var positions = await _chronicDiseasesRepository.GetAllAsync();

            var response = positions.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ChronicDiseaseResponse>> Create([FromBody] CreateChronicDiseaseRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _chronicDiseasesRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ChronicDiseaseResponse>> Update([FromRoute] int id, [FromBody] UpdateChronicDiseaseRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _chronicDiseasesRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _chronicDiseasesRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
