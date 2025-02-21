using Domain.Models.JournalContent;

namespace Domain.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public GroupActive? GroupActive { get; set; }
        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
    }
}
