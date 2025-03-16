using Contracts.Groups;
using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Mappers;

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
        public async Task<ActionResult<List<GroupResponse>>> GetAll()
        {
            var groups = await _groupsRepository.GetAllAsync();

            var response = groups.Select(g => g.ToResponse());
            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult<GroupResponse>> Create([FromBody] CreateGroupRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var group = await _groupsService.CreateGroup(request.Number, request.SpecialtyId, request.AdmissionYear);
            if (group == null) return BadRequest(nameof(group));

            var groupResponse = group.ToResponse();
            return CreatedAtAction(nameof(Create), groupResponse);
        }

        [HttpPut("appointcurator")]
        public async Task<ActionResult> AppointCurator([FromBody] AppointCuratorRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var group = await _groupsService.AppointCurator(request.GroupId, request.CuratorId);
            if (group == null) return BadRequest();
            return Ok();
        }

        [HttpGet("getbyjournal/{journalId}")]
        public async Task<ActionResult<GroupResponse>> GetByJournal([FromRoute] int journalId)
        {
            var group = await _groupsRepository.GetByJournalId(journalId);
            if (group == null) return BadRequest();

            var response = group.ToResponse();
            return Ok(response);
        }
    }
}
