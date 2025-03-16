using API.Mappers;
using Contracts.Departments;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsRepository _departmentsRepository;

        public DepartmentsController(IDepartmentsRepository departmentsRepository)
        {
            _departmentsRepository = departmentsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartmentResponse>>> GetAll()
        {
            var positions = await _departmentsRepository.GetAllAsync();

            var response = positions.Select(l => l.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentResponse>> Create([FromBody] CreateDepartmentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _departmentsRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentResponse>> Update([FromRoute] int id, [FromBody] UpdateDepartmentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _departmentsRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _departmentsRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
