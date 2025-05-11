using API.Mappers;
using Contracts.Journal.Pages;
using DataAccess.Interfaces;
using Domain.Entities.JournalContent.Pages;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/page")]
    [ApiController]
    public class PagesController(IPagesRepository pagesRepository) : ControllerBase
    {
        private readonly IPagesRepository _pagesRepository = pagesRepository;

        [HttpPost]
        public async Task<ActionResult<PageResponse>> AddPage([FromBody] AddPageRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var page = await _pagesRepository.CreateAsync(new Page { JournalId = request.JournalId, PageTypeId = request.PageTypeId, IsApproved = false });
            if (page == null) return BadRequest();

            return CreatedAtAction(nameof(AddPage), new PageResponse(
                page.Id,
                page.JournalId,
                page.IsApproved,
                new PageTypeResponse(page.PageType!.Id, page.PageType.Name, page.PageType.MaxPages, null),
                page.PersonalizedAccountingCard != null && page.PersonalizedAccountingCard.Student != null ? page.PersonalizedAccountingCard.Student.ToResponse() : null));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePage([FromRoute] int id)
        {
            if (!await _pagesRepository.DeleteAsync(id)) return NotFound();
            return NoContent();
        }

        [HttpPut("toggleisapproved/{pageId}")]
        public async Task<ActionResult<bool>> ToggleIsApproved([FromRoute] int pageId)
        {
            var isApproved = await _pagesRepository.ToggleIsApprovedAsync(pageId);
            if (isApproved == null) return NotFound();
            return Ok(isApproved);
        }

        [HttpPut("approvealljournalpages/{journalId}")]
        public async Task<ActionResult> ApproveAllJournalPages([FromRoute] int journalId)
        {
            if (!await _pagesRepository.ApproveAllJournalPagesAsync(journalId)) return NotFound();
            return Ok();
        }

        [HttpPut("Unapprovealljournalpages/{journalId}")]
        public async Task<ActionResult> UnapproveAllJournalPages([FromRoute] int journalId)
        {
            if (!await _pagesRepository.UnapproveAllJournalPagesAsync(journalId)) return NotFound();
            return Ok();
        }
    }
}
