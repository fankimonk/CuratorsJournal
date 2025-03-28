using API.Mappers;
using Contracts.Journal.FinalPerformanceAccounting;
using DataAccess.Interfaces.PageRepositories.FinalPerformanceAccounting;
using Domain.Entities.JournalContent.FinalPerformanceAccounting;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/finalperformanceaccounting")]
    public class FinalPerformanceAccountingController(IPerformanceAccountingRecordsRepository _recordsRepository,
        IPerformanceAccountingColumnsRepository columnsRepository, IPerformanceAccountingGradesRepository gradesRepository) : ControllerBase
    {
        private readonly IPerformanceAccountingRecordsRepository _recordsRepository = _recordsRepository;
        private readonly IPerformanceAccountingColumnsRepository _columnsRepository = columnsRepository;
        private readonly IPerformanceAccountingGradesRepository _gradesRepository = gradesRepository;

        [HttpGet("{pageId}")]
        public async Task<ActionResult<FinalPerformanceAccountingPageResponse>> GetPageData([FromRoute] int pageId)
        {
            var records = await _recordsRepository.GetByPageIdAsync(pageId);
            if (records == null) return BadRequest();

            var certificationTypes = await _columnsRepository.GetByPageIdGroupByCertificationTypes(pageId);
            if (certificationTypes == null) return BadRequest();

            var recordsResponse = records.Select(r => new PerformanceAccountingRecordResponse(
                r.Id,
                r.Number,
                r.StudentId,
                r.PerformanceAccountingGrades
                    .Select(g => new PerformanceAccountingGradeResponse(
                        g.Id, g.IsPassed, g.Grade, g.PerformanceAccountingColumnId))
                    .ToList()
                )
            ).ToList();

            var certificationTypesResponse = certificationTypes.Select(c => c.ToResponse()).ToList();

            var pageResponse = new FinalPerformanceAccountingPageResponse(pageId, recordsResponse, certificationTypesResponse);
            return Ok(pageResponse);
        }

        [HttpPost("record")]
        public async Task<ActionResult<PerformanceAccountingRecordResponse>> AddRecord([FromBody] CreatePerformanceAccountingRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = new FinalPerformanceAccountingRecord
            {
                Number = request.Number,
                PageId = request.PageId,
                StudentId = request.StudentId
            };

            var createdRecord = await _recordsRepository.CreateAsync(record);
            if (createdRecord == null) return BadRequest();

            var response = new PerformanceAccountingRecordResponse(
                createdRecord.Id, 
                createdRecord.Number, 
                createdRecord.StudentId,
                createdRecord.PerformanceAccountingGrades
                    .Select(g => new PerformanceAccountingGradeResponse(
                        g.Id, g.IsPassed, g.Grade, g.PerformanceAccountingColumnId))
                    .ToList()
            );
            return CreatedAtAction(nameof(AddRecord), response);
        }

        [HttpPut("record/{recordId}")]
        public async Task<ActionResult<PerformanceAccountingRecordResponse>> UpdateRecord([FromRoute] int recordId,
            [FromBody] UpdatePerformanceAccountingRecordRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var record = new FinalPerformanceAccountingRecord
            {
                Number = request.Number,
                StudentId = request.StudentId
            };

            var updatedRecord = await _recordsRepository.UpdateAsync(recordId, record);
            if (updatedRecord == null) return BadRequest();

            var response = new PerformanceAccountingRecordResponse(updatedRecord.Id, updatedRecord.Number, updatedRecord.StudentId, null);
            return Ok(response);
        }

        [HttpDelete("record/{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            if (!await _recordsRepository.DeleteAsync(recordId)) return NotFound();

            return NoContent();
        }

        [HttpPost("column")]
        public async Task<ActionResult<PerformanceAccountingColumnResponse>> AddColumn([FromBody] CreatePerformanceAccountingColumnRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var column = new PerformanceAccountingColumn
            {
                CertificationTypeId = (int)request.CertificationTypeId,
                PageId = request.PageId,
                SubjectId = request.SubjectId
            };

            var createdColumn = await _columnsRepository.CreateAsync(column);
            if (createdColumn == null) return BadRequest();

            var response = new PerformanceAccountingColumnResponse(
                createdColumn.Id,
                createdColumn.CertificationTypeId,
                createdColumn.Subject == null ? null : createdColumn.Subject.ToResponse()
            );
            return CreatedAtAction(nameof(AddRecord), response);
        }

        [HttpPut("column/{columnId}")]
        public async Task<ActionResult<PerformanceAccountingColumnResponse>> UpdateColumn([FromRoute] int columnId,
            [FromBody] UpdatePerformanceAccountingColumnRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var column = new PerformanceAccountingColumn
            {
                SubjectId = request.SubjectId
            };

            var updated = await _columnsRepository.UpdateAsync(columnId, column);
            if (updated == null) return BadRequest();

            var response = new PerformanceAccountingColumnResponse(
                updated.Id,
                updated.CertificationTypeId,
                updated.Subject == null ? null : updated.Subject.ToResponse()
            );
            return Ok(response);
        }

        [HttpDelete("column/{columnId}")]
        public async Task<ActionResult> DeleteColumn([FromRoute] int columnId)
        {
            if (!await _columnsRepository.DeleteAsync(columnId)) return NotFound();

            return NoContent();
        }

        [HttpPut("grade/{gradeId}")]
        public async Task<ActionResult<PerformanceAccountingGradeResponse>> UpdateGrade([FromRoute] int gradeId,
            [FromBody] UpdatePerformanceAccountingGradeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var grade = new PerformanceAccountingGrade { IsPassed = request.IsPassed, Grade = request.Grade };

            var updated = await _gradesRepository.UpdateAsync(gradeId, grade);
            if (updated == null) return BadRequest();

            var response = new PerformanceAccountingGradeResponse(
                updated.Id, updated.IsPassed, updated.Grade, updated.PerformanceAccountingColumnId);
            return Ok(response);
        }
    }
}
