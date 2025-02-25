using Contracts.Journal.ContactPhones;
using DataAccess.Interfaces;
using Domain.Entities.JournalContent;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/contactphone")]
    public class ContactPhonesController : ControllerBase
    {
        private readonly IContactPhonesRepository _contactPhonesRepository;

        public ContactPhonesController(IContactPhonesRepository contactPhonesRepository)
        {
            _contactPhonesRepository = contactPhonesRepository;
        }

        [HttpGet("{journalId}")]
        public async Task<ActionResult<ContactPhonesPageResponse>> GetJournalContactPhones([FromRoute] int journalId)
        {
            var phones = await _contactPhonesRepository.GetByJournalIdAsync(journalId);

            var phonesResponse = phones.Select(p => new ContactPhoneResponse(p.Id, p.Name, p.PhoneNumber)).ToList();
            return Ok(new ContactPhonesPageResponse(journalId, phonesResponse));
        }

        [HttpPost]
        public async Task<ActionResult<ContactPhoneResponse>> AddContactPhone([FromBody] CreateContactPhoneRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var phone = new ContactPhoneNumber { Name = request.Name, PhoneNumber = request.PhoneNumber, JournalId = request.JournalId };

            var createdPhone = await _contactPhonesRepository.CreateAsync(phone);
            if (createdPhone == null) return BadRequest();

            var phoneResponse = new ContactPhoneResponse(createdPhone.Id, createdPhone.Name, createdPhone.PhoneNumber);
            return CreatedAtAction(nameof(AddContactPhone), phoneResponse);
        }

        [HttpPut("{phoneId}")]
        public async Task<ActionResult<ContactPhoneResponse>> UpdateContactPhone([FromRoute] int phoneId, [FromBody] UpdateContactPhoneRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var phone = new ContactPhoneNumber { Name = request.Name, PhoneNumber = request.PhoneNumber };
            var updatePhone = await _contactPhonesRepository.UpdateAsync(phoneId, phone);
            if (updatePhone == null) return BadRequest();

            var response = new ContactPhoneResponse(updatePhone.Id, updatePhone.Name, updatePhone.PhoneNumber);
            return Ok(response);
        }

        [HttpDelete("{phoneId}")]
        public async Task<ActionResult> DeleteContactPhone([FromRoute] int phoneId)
        {
            if (!await _contactPhonesRepository.DeleteAsync(phoneId)) return NotFound();

            return NoContent();
        }
    }
}
