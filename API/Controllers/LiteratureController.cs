using API.Mappers;
using Contracts.Literature;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/literature")]
    [ApiController]
    public class LiteratureController : ControllerBase
    {
        private readonly ILiteratureRepository _literatureRepository;

        public LiteratureController(ILiteratureRepository literatureRepository)
        {
            _literatureRepository = literatureRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<LiteratureResponse>>> GetAll()
        {
            var literature = await _literatureRepository.GetAllAsync();

            var response = literature.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<LiteratureResponse>> Create([FromBody] CreateLiteratureRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdLiterature = await _literatureRepository.CreateAsync(request.ToEntity());
            if (createdLiterature == null) return BadRequest();

            var response = createdLiterature.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LiteratureResponse>> Update([FromRoute] int id, [FromBody] UpdateLiteratureRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedLiterature = await _literatureRepository.UpdateAsync(id, request.ToEntity());
            if (updatedLiterature == null) return BadRequest();

            var response = updatedLiterature.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _literatureRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
