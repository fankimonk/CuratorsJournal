using API.Mappers;
using Contracts.Journal.SocioPedagogicalCharacteristics;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/sociopedagogicalcharacteristics")]
    public class SocioPedagogicalCharacteristicsController(
        ISocioPedagogicalCharacteristicsRepository entityRepository) : ControllerBase
    {
        private readonly ISocioPedagogicalCharacteristicsRepository _entityRepository = entityRepository;

        [HttpGet("{pageId}")]
        public async Task<ActionResult<SocioPedagogicalCharacteristicsResponse>> GetByPage([FromRoute] int pageId)
        {
            var characteristics = await _entityRepository.GetByPageIdAsync(pageId);
            if (characteristics == null) return BadRequest();

            var response = characteristics.ToResponse();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SocioPedagogicalCharacteristicsResponse>> Update([FromRoute] int id, 
            [FromBody] UpdateSocioPedagogicalCharacteristicsRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var characteristics = request.ToEntity();
            var updatedCharacteristics = await _entityRepository.UpdateAsync(id, characteristics);
            if (updatedCharacteristics == null) return BadRequest();

            var response = updatedCharacteristics.ToResponse();
            return Ok(response);
        }
    }
}
