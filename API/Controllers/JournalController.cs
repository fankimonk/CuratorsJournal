using Contracts.Journal;
using Contracts.Journal.Pages;
using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Contracts.Curator;

namespace API.Controllers
{
    [Route("api/journal")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IJournalsService _journalsService;
        private readonly IJournalsRepository _journalsRepository;
        private readonly IPagesRepository _pagesRepository;

        public JournalController(IJournalsService journalsService, IJournalsRepository journalsRepository, 
            IPagesRepository pagesRepository)
        {
            _journalsService = journalsService;
            _journalsRepository = journalsRepository;
            _pagesRepository = pagesRepository;
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

        [HttpGet("{journalId}/pages")]
        public async Task<ActionResult<JournalPagesResponse>> GetPages([FromRoute] int journalId)
        {
            var pages = await _pagesRepository.GetByJournalId(journalId);
            if (pages == null) return NotFound();

            var response = pages.Select(p => new PageResponse(
                p.Id,
                p.JournalId,
                new PageTypeResponse(p.PageType!.Id, p.PageType.Name, p.PageType.MaxPages, null)
            )).ToList();

            return Ok(new JournalPagesResponse(journalId, response));
        }

        [HttpGet("{journalId}/contents")]
        public async Task<ActionResult<JournalContentsResponse>> GetContents([FromRoute] int journalId)
        {
            var pages = await _pagesRepository.GetByJournalIdGroupedByTypes(journalId);
            if (pages == null) return NotFound();

            var response = pages.Select(pt => new PageTypeResponse(
                pt.Id,
                pt.Name,
                pt.MaxPages,
                pt.Pages.Select(p => new PageResponse(p.Id, p.JournalId, new PageTypeResponse(pt.Id, pt.Name, pt.MaxPages, null))).ToList()
            )).ToList();

            return Ok(new JournalContentsResponse(journalId, response));
        }

        [HttpGet("{journalId}/titlepage")]
        public async Task<ActionResult<TitlePageResponse>> GetTitlePage([FromRoute] int journalId)
        {
            var titlePageData = await _journalsService.GetJournalsTitlePageData(journalId);
            if (titlePageData == null) return NotFound(nameof(journalId));

            var journal = titlePageData.Item2;

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
                titlePageData.Item1,
                journalId,
                group.Number,
                group.AdmissionYear.ToString(),
                curatorResponse,
                department!.AbbreviatedName,
                department!.Deanery!.Faculty!.Name
            );

            return Ok(titlePageResponse);
        }
    }
}
