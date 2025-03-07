using API.Mappers;
using API.Mappers.Journal;
using API.Mappers.Journal.PersonalizedAccountingCards;
using Contracts.Journal.PersonalizedAccountingCards;
using DataAccess.Interfaces.PageRepositories.PersonalizedAccountingCards;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/personalizedaccountingcard")]
    public class PersonalizedAccountingCardsController(IPersonalizedAccountingCardsRepository entityRepository) : ControllerBase
    {
        private readonly IPersonalizedAccountingCardsRepository _entityRepository = entityRepository;

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
    }
}
