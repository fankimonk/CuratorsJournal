using Contracts.Group;
using Application.Interfaces;
using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<Group>>> GetAll()
        {
            var groups = await _groupsRepository.GetAllAsync();

            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateGroupRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var group = await _groupsService.CreateGroup(request.Number, request.SpecialtyId, request.AdmissionYear);
            if (group == null) return BadRequest(nameof(group));
            return Ok();
        }

        [HttpPut("appointcurator")]
        public async Task<ActionResult> AppointCurator([FromBody] AppointCuratorRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var group = await _groupsService.AppointCurator(request.GroupId, request.CuratorId);
            if (group == null) return BadRequest();
            return Ok();
        }

    }
}
