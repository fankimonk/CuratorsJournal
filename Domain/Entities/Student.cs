using Domain.Entities.JournalContent;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
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

        public int? UserId { get; set; }
        public User? User { get; set; }

        public GroupActive? GroupActive { get; set; }
        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
        public StudentListRecord? StudentListRecord { get; set; }

        public List<ChronicDisease> ChronicDiseases { get; set; } = [];
        public List<PEGroup> PEGroups { get; set; } = [];
        public List<StudentsHealthCardRecord> StudentsHealthCardRecords { get; set; } = [];
        public List<FinalPerformanceAccountingRecord> FinalPerformanceAccountingRecords { get; set; } = [];
    }
}
