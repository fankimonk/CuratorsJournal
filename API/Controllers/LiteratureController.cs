using API.Mappers;
using Contracts.Literature;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/literature")]
    [ApiController]
    public class LiteratureController : ControllerBase
    {
        private readonly ILiteratureRepository _literatureRepository;

        public LiteratureController(ILiteratureRepository literatureRepository)
        {
            _literatureRepository = literatureRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<LiteratureResponse>>> GetAll()
        {
            var literature = await _literatureRepository.GetAllAsync();

            var response = literature.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }
    }
}
