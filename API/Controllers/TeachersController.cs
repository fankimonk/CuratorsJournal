using API.Mappers;
using Contracts.Teachers;
using Contracts.Workers;
using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly IWorkersRepository _workersRepository;
        private readonly ITeachersRepository _teachersRepository;

        public TeachersController(IWorkersRepository workersRepository, ITeachersRepository teachersRepository)
        {
            _workersRepository = workersRepository;
            _teachersRepository = teachersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeacherResponse>>> GetAll()
        {
            var teachers = await _teachersRepository.GetAllAsync();

            var response = teachers.Select(w => new TeacherResponse(w.Id, w.Worker!.FirstName, w.Worker.MiddleName, w.Worker.LastName, w.DepartmentId));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<TeacherResponse>> Create([FromBody] CreateTeacherRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdWorker = await _workersRepository.CreateAsync(
                new Worker { 
                    FirstName = request.FirstName, 
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    PositionId = 4
                });
            if (createdWorker == null) return BadRequest();

            var createdTeacher = await _teachersRepository.CreateAsync(new Teacher { WorkerId = createdWorker.Id, DepartmentId = (int)request.DepartmentId });
            if (createdTeacher == null) return BadRequest();

            var response = new TeacherResponse(createdTeacher.Id, createdWorker.FirstName, createdWorker.MiddleName, createdWorker.LastName, createdTeacher.DepartmentId);

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TeacherResponse>> Update([FromRoute] int id, [FromBody] UpdateTeacherRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedTeacher = await _teachersRepository.UpdateAsync(id, new Teacher { DepartmentId = request.DepartmentId });
            if (updatedTeacher == null) return BadRequest();

            var updatedWorker = await _workersRepository.UpdateAsync(updatedTeacher.WorkerId, 
                new Worker { 
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    PositionId = 4
                });
            if (updatedWorker == null) return BadRequest();

            var response = new TeacherResponse(updatedTeacher.Id, updatedWorker.FirstName, updatedWorker.MiddleName, updatedWorker.LastName, updatedTeacher.DepartmentId);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var teacher = await _teachersRepository.GetByIdAsync(id);
            if (teacher == null) return NotFound();

            if (!await _workersRepository.DeleteAsync(teacher.WorkerId)) return NotFound();

            return NoContent();
        }
    }
}
