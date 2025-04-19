using API.Mappers;
using API.Mappers.Journal;
using API.Mappers.Journal.PersonalizedAccountingCards;
using Application.Interfaces;
using Contracts.Journal.PersonalizedAccountingCards;
using Contracts.Journal.StudentList;
using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/personalizedaccountingcard")]
    [ApiController]
    public class PersonalizedAccountingCardsController(IPersonalizedAccountingCardsRepository entityRepository,
        IPersonalizedAccountingCardsService entityService) : ControllerBase
    {
        private readonly IPersonalizedAccountingCardsRepository _entityRepository = entityRepository;
        private readonly IPersonalizedAccountingCardsService _entityService = entityService;

        [HttpGet("{pageId}")]
        public async Task<ActionResult<PersonalizedAccountingCardResponse>> GetByPage([FromRoute] int pageId)
        {
            var card = await _entityRepository.GetByPageIdAsync(pageId);
            if (card == null) return BadRequest();

            var response = card.ToResponse();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PersonalizedAccountingCardResponse>> Update([FromRoute] int id, 
            [FromBody] UpdatePersonalizedAccountingCardRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var card = request.ToEntity();
            var updatedCard = await _entityRepository.UpdateAsync(id, card);
            if (updatedCard == null) return BadRequest();

            var response = updatedCard.ToResponse();
            return Ok(response);
        }

        [HttpGet("getidsbyjournal/{journalId}")]
        public async Task<ActionResult<List<PersonalizedAccountingCardIdResponse>>> GetIdsByJournal([FromRoute] int journalId)
        {
            var cards = await _entityRepository.GetByJournalId(journalId);

            var response = cards.Select(c => new PersonalizedAccountingCardIdResponse(c.Id));
            return Ok(response);
        }

        [HttpGet("getbystudent/{studentId}")]
        public async Task<ActionResult<int>> GetCardByStudentId([FromRoute] int studentId)
        {
            var card = await _entityRepository.GetCardByStudentIdAsync(studentId);
            if (card == null) return NotFound();
            return Ok(new CardInfoResponse(card.Id, card.PageId));
        }

        [HttpGet("getstudentswithcards/{journalId}")]
        public async Task<ActionResult<List<int>>> GetStudentIdsThatHaveCards([FromRoute] int journalId)
        {
            var ids = await _entityRepository.GetStudentIdsThatHaveCard(journalId);
            return Ok(ids);
        }

        [HttpPost("synchronizestudents/{journalId}")]
        public async Task<ActionResult> SynchronizeStudents([FromRoute] int journalId)
        {
            var studentIdsWithoutCards = await _entityService.SynchronizeStundets(journalId);
            if (studentIdsWithoutCards == null) return BadRequest();
            return Ok(studentIdsWithoutCards);
        }
    }
}
