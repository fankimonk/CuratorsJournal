using API.Mappers;
using Contracts.CertificationTypes;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/certificationtype")]
    [ApiController]
    public class CertificationTypesController : ControllerBase
    {
        private readonly ICertificationTypesRepository _certificationTypesRepository;

        public CertificationTypesController(ICertificationTypesRepository certificationTypesRepository)
        {
            _certificationTypesRepository = certificationTypesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<CertificationTypeResponse>>> GetAll()
        {
            var positions = await _certificationTypesRepository.GetAllAsync();

            var response = positions.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CertificationTypeResponse>> Create([FromBody] CreateCertificationTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _certificationTypesRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CertificationTypeResponse>> Update([FromRoute] int id, [FromBody] UpdateCertificationTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _certificationTypesRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _certificationTypesRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
