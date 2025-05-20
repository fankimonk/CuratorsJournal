using API.Mappers;
using Contracts.AcademicYears;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/academicyear")]
    [ApiController]
    public class AcademicYearsController : ControllerBase
    {
        private readonly IAcademicYearsRepository _academicYearsRepository;

        public AcademicYearsController(IAcademicYearsRepository academicYearsRepository)
        {
            _academicYearsRepository = academicYearsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<AcademicYearResponse>>> GetAll()
        {
            var academicYears = await _academicYearsRepository.GetAllAsync();

            var response = academicYears.Select(a => a.ToResponse());
            return Ok(response);
        }

        [HttpGet("getsinceyear/{yearSince}")]
        public async Task<ActionResult<List<AcademicYearResponse>>> GetAllSinceYear(int yearSince)
        {
            var academicYears = await _academicYearsRepository.GetAllAsync(yearSince);

            var response = academicYears.Select(a => a.ToResponse());
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<AcademicYearResponse>> Create([FromBody] CreateAcademicYearRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _academicYearsRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _academicYearsRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
