using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            throw new NotImplementedException();
        }
    }
}
