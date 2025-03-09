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
            var academicYears = await _academicYearsRepository.GetAll();

            var response = academicYears.Select(a => a.ToResponse());
            return Ok(response);
        }

        [HttpGet("getsinceyear/{yearSince}")]
        public async Task<ActionResult<List<AcademicYearResponse>>> GetAllSinceYear(int yearSince)
        {
            var academicYears = await _academicYearsRepository.GetAll(yearSince);

            var response = academicYears.Select(a => a.ToResponse());
            return Ok(response);
        }
    }
}
