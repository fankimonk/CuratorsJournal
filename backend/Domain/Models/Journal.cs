using Domain.Models.JournalContent;

namespace Domain.Models
{
    public class Journal
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public List<ContactPhoneNumber> ContactPhoneNumbers { get; set; } = [];
        public List<SocioPedagogicalCharacteristics> SocioPedagogicalCharacteristics { get; set; } = [];
        public List<GroupActive> GroupActives { get; set; } = [];
        public List<PersonalizedAccountingCard> PersonalizedAccountingCards { get; set; } = [];

        public EducationalProcessSchedule? EducationalProcessSchedule { get; set; }
    }
}
