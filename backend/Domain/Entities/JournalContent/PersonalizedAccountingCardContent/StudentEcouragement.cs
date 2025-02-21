namespace Domain.Entities.JournalContent.PersonalizedAccountingCardContent
{
    public class StudentEcouragement
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public string Achievement { get; set; } = string.Empty;
        public string EncouragementKind { get; set; } = string.Empty;

        public int PersonalizedAccountingCardId { get; set; }
        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
    }
}
