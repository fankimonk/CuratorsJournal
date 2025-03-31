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

            var page = await _pagesRepository.CreateAsync(new Page { JournalId = request.JournalId, PageTypeId = request.PageTypeId });
            if (page == null) return BadRequest();

            return CreatedAtAction(nameof(AddPage), new PageResponse(
                page.Id,
                page.JournalId,
                new PageTypeResponse(page.PageType!.Id, page.PageType.Name, page.PageType.MaxPages, null)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePage([FromRoute] int id)
        {
            if (!await _pagesRepository.DeleteAsync(id)) return NotFound();
            return NoContent();
        }
    }
}
