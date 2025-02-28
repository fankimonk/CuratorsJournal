using Contracts.Journal.ContactPhones;
using DataAccess.Interfaces;
using Domain.Entities.JournalContent;
using Domain.Enums.Journal;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/journal/contactphone")]
    public class ContactPhonesController : ControllerBase
    {
        private readonly IContactPhonesRepository _contactPhonesRepository;
        private readonly IPagesRepository _pagesRepository;

        public ContactPhonesController(IContactPhonesRepository contactPhonesRepository, IPagesRepository pagesRepository)
        {
            _contactPhonesRepository = contactPhonesRepository;
            _pagesRepository = pagesRepository;
        }

        [HttpGet("{pageId}")]
        public async Task<ActionResult<ContactPhonesPageResponse>> GetContactPhonesByPage([FromRoute] int pageId)
        {
            var phones = await _contactPhonesRepository.GetByPageIdAsync(pageId);

            var phonesResponse = phones.Select(p => new ContactPhoneResponse(p.Id, p.Name, p.PhoneNumber)).ToList();
            return Ok(new ContactPhonesPageResponse(pageId, phonesResponse));
        }

        [HttpPost]
        public async Task<ActionResult<ContactPhoneResponse>> AddContactPhone([FromBody] CreateContactPhoneRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var page = await _pagesRepository.GetById(request.PageId);
            if (page == null || page.PageTypeId != (int)PageTypes.ContactPhonesPage) return BadRequest(nameof(request.PageId));

            var phone = new ContactPhoneNumber { Name = request.Name, PhoneNumber = request.PhoneNumber, PageId = request.PageId };

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
