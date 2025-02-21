using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.JournalContent
{
    public class ContactPhoneNumber
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [MinLength(9)]
        [MaxLength(17)]
        public string PhoneNumber { get; set; } = string.Empty;

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
