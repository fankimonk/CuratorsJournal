using API.Mappers;
using Contracts.Journal.GroupActives;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/groupactive")]
    public class GroupActivesController : ControllerBase
    {
        private readonly IGroupActivesRepository _groupActivesRepository;

        public GroupActivesController(IGroupActivesRepository groupActivesRepository)
        {
            _groupActivesRepository = groupActivesRepository;
        }

        [HttpGet("{pageId}")]
        public async Task<ActionResult<GroupActivesPageResponse>> GetByPage([FromRoute] int pageId)
        {
            var actives = await _groupActivesRepository.GetByPageIdAsync(pageId);
            if (actives == null) return BadRequest();

            var response = actives.Select(p => p.ToResponse()).ToList();
            return Ok(new GroupActivesPageResponse(pageId, response));
        }

        [HttpPost]
        public async Task<ActionResult<GroupActiveResponse>> AddGroupActive([FromBody] CreateGroupActiveRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var active = request.ToEntity();

            var createdActive = await _groupActivesRepository.CreateAsync(active);
            if (createdActive == null) return BadRequest();

            var response = createdActive.ToResponse();
            return CreatedAtAction(nameof(AddGroupActive), response);
        }

        [HttpPut("{activeId}")]
        public async Task<ActionResult<GroupActiveResponse>> UpdateGroupActive([FromRoute] int activeId, [FromBody] UpdateGroupActiveRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var active = request.ToEntity();
            var updateActive = await _groupActivesRepository.UpdateAsync(activeId, active);
            if (updateActive == null) return BadRequest();

            var response = updateActive.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{activeId}")]
        public async Task<ActionResult> DeleteGroupActive([FromRoute] int activeId)
        {
            if (!await _groupActivesRepository.DeleteAsync(activeId)) return NotFound();

            return NoContent();
        }
    }
}
