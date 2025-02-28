namespace Contracts.Journal.ContactPhones
{
    public class ContactPhonesPageResponse(int pageId, List<ContactPhoneResponse> phoneNumbers)
    {
        public int PageId { get; set; } = pageId;
        public List<ContactPhoneResponse> PhoneNumbers { get; set; } = phoneNumbers;
    }

    public class ContactPhoneResponse(int id, string name, string phoneNumber)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string PhoneNumber { get; set; } = phoneNumber;
    }
}
