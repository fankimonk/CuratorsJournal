using API.Contracts.Journal;
using Application.Interfaces;
using Application.Models;
using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [Route("api/journal")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IJournalsService _journalsService;
        private readonly IJournalsRepository _journalsRepository;

        public JournalController(IJournalsService journalsService, IJournalsRepository journalsRepository)
        {
            _journalsService = journalsService;
            _journalsRepository = journalsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<JournalResponse>>> GetAll()
        {
            var journals = await _journalsRepository.GetAllAsync();

            var journalResponses = journals.Select(j =>
            {
                var group = j.Group;
                var curator = group!.Curator;
                (string, string, string)? curatorFio = curator == null ? null : (
                    group.Curator!.Teacher!.Worker!.LastName,
                    group.Curator.Teacher.Worker.FirstName,
                    group.Curator.Teacher.Worker.MiddleName
                );

                return new JournalResponse(j.Id, group.Number, curatorFio);
            }).ToList();

            return Ok(journalResponses);
        }

        [HttpGet]
        [Route("titlepage/{journalId}")]
        public async Task<ActionResult<TitlePageResponse>> GetTitlePage([FromRoute] int journalId)
        {
            var journal = await _journalsService.GetJournalsTitlePageData(journalId);
            if (journal == null) return NotFound(nameof(journalId));

            var group = journal.Group;
            var department = group!.Specialty!.Department;

            (string, string, string)? curatorFio = group.Curator == null ? null : (
                group.Curator.Teacher!.Worker!.LastName,
                group.Curator.Teacher.Worker.FirstName,
                group.Curator.Teacher.Worker.MiddleName
            );

            var titlePageResponse = new TitlePageResponse(
                journalId,
                group.Number,
                group.AdmissionYear.ToString(),
                curatorFio,
                department!.AbbreviatedName,
                department!.Deanery!.Faculty!.AbbreviatedName
            );

            return Ok(titlePageResponse);
        }
    }
}
