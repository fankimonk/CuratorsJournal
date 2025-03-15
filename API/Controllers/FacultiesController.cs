using API.Mappers;
using Contracts.Faculties;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/faculty")]
    [ApiController]
    public class FacultiesController : ControllerBase
    {
        private readonly IFacultiesRepository _facultiesRepository;

        public FacultiesController(IFacultiesRepository facultiesRepository)
        {
            _facultiesRepository = facultiesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<FacultyResponse>>> GetAll()
        {
            var literature = await _facultiesRepository.GetAllAsync();

            var response = literature.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<FacultyResponse>> Create([FromBody] CreateFacultyRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdFaculty = await _facultiesRepository.CreateAsync(request.ToEntity());
            if (createdFaculty == null) return BadRequest();

            var response = createdFaculty.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FacultyResponse>> Update([FromRoute] int id, [FromBody] UpdateFacultyRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedFaculty = await _facultiesRepository.UpdateAsync(id, request.ToEntity());
            if (updatedFaculty == null) return BadRequest();

            var response = updatedFaculty.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _facultiesRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
