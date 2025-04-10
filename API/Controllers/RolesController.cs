using Contracts.User;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RolesController(IRolesRepository rolesRepository) : ControllerBase
    {
        private readonly IRolesRepository _rolesRepository = rolesRepository;

        [HttpGet]
        public async Task<ActionResult<List<RoleResponse>>> GetAll()
        {
            return Ok(await _rolesRepository.GetAllAsync());
        }
    }
}
