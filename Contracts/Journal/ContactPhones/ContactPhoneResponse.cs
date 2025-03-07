namespace Contracts.Journal.ContactPhones
{
    public class ContactPhoneResponse(int id, string? name, string? phoneNumber)
    {
        public int Id { get; set; } = id;
        public string? Name { get; set; } = name;
        public string? PhoneNumber { get; set; } = phoneNumber;
    }
}
