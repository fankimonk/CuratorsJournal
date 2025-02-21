namespace Domain.Models.JournalContent
{
    public class ContactPhoneNumber
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
