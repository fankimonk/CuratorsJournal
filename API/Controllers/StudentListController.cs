using API.Mappers;
using Contracts.Journal.StudentList;
using DataAccess.Interfaces.PageRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/studentlist")]
    public class StudentListController : ControllerBase
    {
        private readonly IStudentListRepository _studentListRepository;

        public StudentListController(IStudentListRepository studentListRepository)
        {
            _studentListRepository = studentListRepository;
        }

        [HttpGet("{pageId}")]
        public async Task<ActionResult<StudentListPageResponse>> GetByPage([FromRoute] int pageId)
        {
            var records = await _studentListRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var response = records.Select(p => p.ToResponse()).ToList();
            return Ok(new StudentListPageResponse(pageId, response));
        }

        [HttpPost]
        public async Task<ActionResult<StudentListRecordResponse>> AddRecord([FromBody] CreateStudentListRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();

            var createdRecord = await _studentListRepository.CreateAsync(record);
            if (createdRecord == null) return BadRequest();

            var response = createdRecord.ToResponse();
            return CreatedAtAction(nameof(AddRecord), response);
        }

        [HttpPut("{recordId}")]
        public async Task<ActionResult<StudentListRecordResponse>> UpdateContactPhone([FromRoute] int recordId, [FromBody] UpdateStudentListRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = request.ToEntity();
            var updatedRecord = await _studentListRepository.UpdateAsync(recordId, record);
            if (updatedRecord == null) return BadRequest();

            var response = updatedRecord.ToResponse();
            return Ok(response);
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteContactPhone([FromRoute] int recordId)
        {
            if (!await _studentListRepository.DeleteAsync(recordId)) return NotFound();

            return NoContent();
        }
    }
}
