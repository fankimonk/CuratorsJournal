using API.Mappers;
using Contracts.Students;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsRepository _studentsRepository;

        public StudentsController(IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudentResponse>>> GetAll()
        {
            var students = await _studentsRepository.GetAllAsync();

            var response = students.Select(s => s.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpGet("getbygroup/{groupId}")]
        public async Task<ActionResult<List<StudentResponse>>> GetByGroup([FromRoute] int groupId)
        {
            var students = await _studentsRepository.GetByGroupIdAsync(groupId);
            if (students == null) return BadRequest();

            var response = students.Select(s => s.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpGet("getbyjournal/{journalId}")]
        public async Task<ActionResult<List<StudentResponse>>> GetByJournal([FromRoute] int journalId)
        {
            var students = await _studentsRepository.GetByJournalIdAsync(journalId);
            if (students == null) return BadRequest();

            var response = students.Select(s => s.ToResponse()).ToList();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<StudentResponse>> Create([FromBody] CreateStudentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _studentsRepository.CreateAsync(request.ToEntity());
            if (created == null) return BadRequest();

            var response = created.ToResponse();

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StudentResponse>> Update([FromRoute] int id, [FromBody] UpdateStudentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _studentsRepository.UpdateAsync(id, request.ToEntity());
            if (updated == null) return BadRequest();

            var response = updated.ToResponse();

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            if (!await _studentsRepository.DeleteAsync(id)) return NotFound();

            return NoContent();
        }
    }
}
