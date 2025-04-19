using Contracts.Journal;
using Contracts.Journal.Pages;
using Application.Interfaces;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Contracts.Curator;
using Microsoft.AspNetCore.Authorization;
using Application.Authorization;
using Domain.Entities;
using API.Mappers;

namespace API.Controllers
{
    [Route("api/journal")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IJournalsService _journalsService;
        private readonly IJournalsRepository _journalsRepository;
        private readonly IPagesRepository _pagesRepository;
        private readonly IWordService _wordService;

        public JournalController(IJournalsService journalsService, IJournalsRepository journalsRepository, 
            IPagesRepository pagesRepository, IWordService wordService)
        {
            _journalsService = journalsService;
            _journalsRepository = journalsRepository;
            _pagesRepository = pagesRepository;
            _wordService = wordService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<JournalResponse>>> GetAll()
        {
            var userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId)!.Value);

            var journals = await _journalsRepository.GetAllAsync(userId);
            if (journals == null) return BadRequest();

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
                        curator!.Worker!.FirstName,
                        curator.Worker.MiddleName,
                        curator.Worker.LastName
                    );
                }

                return new JournalResponse(j.Id, group.Number, curatorResponse);
            }).ToList();

            return Ok(journalResponses);
        }

        [HttpGet("verifyaccess/{journalId}")]
        [Authorize]
        public async Task<ActionResult<bool>> VerifyAccess([FromRoute] int journalId)
        {
            var userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId)!.Value);
            return Ok(await _journalsRepository.VerifyAccess(userId, journalId));
        }

        [HttpGet("downloadword/{journalId}")]
        public async Task<IActionResult> DownloadWord([FromRoute] int journalId)
        {
            var fileData = await _wordService.GenerateWord(journalId);
            if (fileData == null) return BadRequest();

            return File(fileData.MemoryStream, fileData.ContentType, fileData.FileName);
        }

        [HttpGet("{journalId}/pages")]
        public async Task<ActionResult<JournalPagesResponse>> GetPages([FromRoute] int journalId)
        {
            var pages = await _pagesRepository.GetByJournalId(journalId);
            if (pages == null) return NotFound();

            var response = pages.Select(p => new PageResponse(
                p.Id,
                p.JournalId,
                p.IsApproved,
                new PageTypeResponse(p.PageType!.Id, p.PageType.Name, p.PageType.MaxPages, null),
                p.PersonalizedAccountingCard != null && p.PersonalizedAccountingCard.Student != null ? p.PersonalizedAccountingCard.Student.ToResponse() : null
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
                pt.Pages.Select(p => new PageResponse(
                    p.Id, 
                    p.JournalId, 
                    p.IsApproved, 
                    new PageTypeResponse(pt.Id, pt.Name, pt.MaxPages, null),
                    p.PersonalizedAccountingCard != null && p.PersonalizedAccountingCard.Student != null ? p.PersonalizedAccountingCard.Student.ToResponse() : null)).ToList()
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
                    curator!.Worker!.FirstName,
                    curator.Worker.MiddleName,
                    curator.Worker.LastName
                );
            }

            var titlePageResponse = new TitlePageResponse(
                titlePageData.Item1,
                journalId,
                group.ToResponse(),
                curatorResponse,
                department!.AbbreviatedName,
                department!.Deanery!.Faculty!.Name
            );

            return Ok(titlePageResponse);
        }
    }
}
