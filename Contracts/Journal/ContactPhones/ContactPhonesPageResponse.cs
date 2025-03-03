namespace Contracts.Journal.ContactPhones
{
    public class ContactPhonesPageResponse(int pageId, List<ContactPhoneResponse> phoneNumbers)
    {
        public int PageId { get; set; } = pageId;
        public List<ContactPhoneResponse> PhoneNumbers { get; set; } = phoneNumbers;
    }
}
