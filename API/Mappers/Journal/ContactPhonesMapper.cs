using Contracts.Journal.ContactPhones;
using Domain.Entities.JournalContent;

namespace API.Mappers.Journal
{
    public static class ContactPhonesMapper
    {
        public static ContactPhoneResponse ToResponse(this ContactPhoneNumber phone)
        {
            return new ContactPhoneResponse(
                phone.Id, phone.Name, phone.PhoneNumber
            );
        }

        public static ContactPhoneNumber ToEntity(this UpdateContactPhoneRequest request)
        {
            return new ContactPhoneNumber
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };
        }

        public static ContactPhoneNumber ToEntity(this CreateContactPhoneRequest request)
        {
            return new ContactPhoneNumber
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                PageId = request.PageId
            };
        }
    }
}
