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
    }
}
