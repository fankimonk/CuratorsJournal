using API.Mappers;
using Contracts.ActivityTypes;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/activitytype")]
    [ApiController]
    public class ActivityTypesController : ControllerBase
    {
        private readonly IActivityTypesRepository _activityTypesRepository;

        public ActivityTypesController(IActivityTypesRepository activityTypesRepository)
        {
            _activityTypesRepository = activityTypesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActivityTypeResponse>>> GetAll()
        {
            var positions = await _activityTypesRepository.GetAllAsync();

            var response = positions.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ActivityTypeResponse>> Create([FromBody] CreateActivityTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _activityTypesRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ActivityTypeResponse>> Update([FromRoute] int id, [FromBody] UpdateActivityTypeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _activityTypesRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _activityTypesRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
