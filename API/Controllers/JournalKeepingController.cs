using Application.Entities;
using Application.Interfaces;
using Contracts.JournalKeeping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/keeping")]
    [ApiController]
    public class JournalKeepingController(IJournalKeepingService journalKeepingService) : ControllerBase
    {
        private readonly IJournalKeepingService _journalKeepingService = journalKeepingService;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<JournalKeepingResponse>> Get()
        {
            var journalKeeping = await _journalKeepingService.Get();
            if (journalKeeping == null) return StatusCode(500, "File doesn't exist");
            return Ok(new JournalKeepingResponse(journalKeeping.Title, journalKeeping.Content));
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<JournalKeepingResponse>> Update([FromBody] UpdateJournalKeepingRequest request)
        {
            var updated = await _journalKeepingService.Update(new JournalKeeping(request.Title, request.Content));
            if (updated == null) return StatusCode(500);
            return Ok(new JournalKeepingResponse(updated.Title, updated.Content));
        }
    }
}
