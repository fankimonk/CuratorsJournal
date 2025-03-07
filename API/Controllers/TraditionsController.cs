using API.Mappers;
using API.Mappers.Journal;
using Contracts.Journal.ContactPhones;
using Contracts.Journal.Traditions;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/tradition")]
    public class TraditionsController : ControllerBase
    {
        private readonly ITraditionsRepository _traditionsRepository;

        public TraditionsController(ITraditionsRepository traditionsRepository)
        {
            _traditionsRepository = traditionsRepository;
        }

        [HttpGet("{pageId}")]
        public async Task<ActionResult<TraditionsPageResponse>> GetByPage([FromRoute] int pageId)
        {
            var traditions = await _traditionsRepository.GetByPageIdAsync(pageId);
            if (traditions == null) return BadRequest();

            var response = traditions.Select(p => p.ToResponse()).ToList();
            return Ok(new TraditionsPageResponse(pageId, response));
        }

        [HttpPost]
        public async Task<ActionResult<TraditionResponse>> Add([FromBody] CreateTraditionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tradition = request.ToEntity();

            var createdTradition = await _traditionsRepository.CreateAsync(tradition);
            if (createdTradition == null) return BadRequest();

            var response = createdTradition.ToResponse();
            return CreatedAtAction(nameof(Add), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TraditionResponse>> Update([FromRoute] int id, [FromBody] UpdateTraditionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tradition = request.ToEntity();
            var updatedTradition = await _traditionsRepository.UpdateAsync(id, tradition);
            if (updatedTradition == null) return BadRequest();

            var response = updatedTradition.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _traditionsRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
