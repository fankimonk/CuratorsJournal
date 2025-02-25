using Contracts.Journal;
using Application.Interfaces;
using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Contracts.Curator;

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
                CuratorResponse? curatorResponse = null;

                if (curator != null)
                {
                    curatorResponse = new CuratorResponse
                    (
                        curator.Id,
                        curator.Teacher!.Worker!.FirstName,
                        curator.Teacher.Worker.MiddleName,
                        curator.Teacher.Worker.LastName
                    );
                }

                return new JournalResponse(j.Id, group.Number, curatorResponse);
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

            var curator = group.Curator;
            CuratorResponse? curatorResponse = null;

            if (curator != null)
            {
                curatorResponse = new CuratorResponse
                (
                    curator.Id,
                    curator.Teacher!.Worker!.FirstName,
                    curator.Teacher.Worker.MiddleName,
                    curator.Teacher.Worker.LastName
                );
            }

            var titlePageResponse = new TitlePageResponse(
                journalId,
                group.Number,
                group.AdmissionYear.ToString(),
                curatorResponse,
                department!.AbbreviatedName,
                department!.Deanery!.Faculty!.AbbreviatedName
            );

            return Ok(titlePageResponse);
        }
    }
}
