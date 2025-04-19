using Contracts.Groups;
using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Mappers;
using Microsoft.AspNetCore.Authorization;
using Application.Authorization;
using Contracts.Curator;

namespace API.Controllers
{
    [Route("api/group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupsService _groupsService;
        private readonly IGroupsRepository _groupsRepository;

        public GroupController(IGroupsService groupsService, IGroupsRepository groupsRepository)
        {
            _groupsService = groupsService;
            _groupsRepository = groupsRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<GroupResponse>>> GetAll()
        {
            var userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId)!.Value);

            var groups = await _groupsRepository.GetAllAsync(userId);
            if (groups == null) return BadRequest();

            var response = groups.Select(g => new GroupResponse(g.Id, g.Number, g.AdmissionYear, g.SpecialtyId, g.CuratorId)).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GroupResponse>> Create([FromBody] CreateGroupRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var group = await _groupsService.CreateGroup(request.Number, (int)request.SpecialtyId, (int)request.AdmissionYear, request.CuratorId);
            if (group == null) return BadRequest(nameof(group));

            var groupResponse = group.ToResponse();
            return CreatedAtAction(nameof(Create), groupResponse);
        }

        [HttpPut("appointcurator")]
        public async Task<ActionResult<CuratorResponse?>> AppointCurator([FromBody] AppointCuratorRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var group = await _groupsService.AppointCurator(request.GroupId, request.CuratorId);
            if (group == null) return BadRequest();
            if (group.Curator != null)
            {
                var worker = group.Curator.Worker;
                return Ok(new CuratorResponse(group.Curator.Id, worker.FirstName, worker.MiddleName, worker.LastName));
            }
            return Ok(null);
        }

        [HttpGet("getbyjournal/{journalId}")]
        public async Task<ActionResult<GroupResponse>> GetByJournal([FromRoute] int journalId)
        {
            var group = await _groupsRepository.GetByJournalId(journalId);
            if (group == null) return BadRequest();

            var response = group.ToResponse();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GroupResponse>> Update([FromRoute] int id, [FromBody] UpdateGroupRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _groupsRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _groupsRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
