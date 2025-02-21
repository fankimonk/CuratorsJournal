using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace Domain.Entities
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<IndividualInformationRecord> IndividualInformationRecords { get; set; } = [];
    }
}
