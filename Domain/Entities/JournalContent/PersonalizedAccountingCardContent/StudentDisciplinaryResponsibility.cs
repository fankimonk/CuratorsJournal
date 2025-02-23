namespace Domain.Entities.JournalContent.PersonalizedAccountingCardContent
{
    public class StudentDisciplinaryResponsibility
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public string Misdemeanor { get; set; } = string.Empty;
        public string DisciplinaryResponsibilityKind { get; set; } = string.Empty;

        public int PersonalizedAccountingCardId { get; set; }
        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
    }
}
