using API.Mappers;
using Contracts.Journal.ContactPhones;
using DataAccess.Interfaces;
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

        [HttpGet("{pageId}")]
        public async Task<ActionResult<ContactPhonesPageResponse>> GetContactPhonesByPage([FromRoute] int pageId)
        {
            var phones = await _contactPhonesRepository.GetByPageIdAsync(pageId);
            if (phones == null) return BadRequest();

            var phonesResponse = phones.Select(p => p.ToResponse()).ToList();
            return Ok(new ContactPhonesPageResponse(pageId, phonesResponse));
        }

        [HttpPost]
        public async Task<ActionResult<ContactPhoneResponse>> AddContactPhone([FromBody] CreateContactPhoneRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var phone = request.ToEntity();

            var createdPhone = await _contactPhonesRepository.CreateAsync(phone);
            if (createdPhone == null) return BadRequest();

            var phoneResponse = createdPhone.ToResponse();
            return CreatedAtAction(nameof(AddContactPhone), phoneResponse);
        }

        [HttpPut("{phoneId}")]
        public async Task<ActionResult<ContactPhoneResponse>> UpdateContactPhone([FromRoute] int phoneId, [FromBody] UpdateContactPhoneRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var phone = request.ToEntity();
            var updatedPhone = await _contactPhonesRepository.UpdateAsync(phoneId, phone);
            if (updatedPhone == null) return BadRequest();

            var response = updatedPhone.ToResponse();
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
