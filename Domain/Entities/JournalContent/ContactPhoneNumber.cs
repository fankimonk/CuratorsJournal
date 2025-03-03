using Domain.Entities.JournalContent.Pages;

namespace Domain.Entities.JournalContent
{
    public class ContactPhoneNumber
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public int PageId { get; set; }
        public Page? Page;
    }
}
