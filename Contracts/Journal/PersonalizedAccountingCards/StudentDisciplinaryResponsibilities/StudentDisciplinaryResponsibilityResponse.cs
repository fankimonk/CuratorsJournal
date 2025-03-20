namespace Contracts.Journal.PersonalizedAccountingCards.StudentDisciplinaryResponsibilities
{
    public class StudentDisciplinaryResponsibilityResponse(
        int id, DateOnly? date, string? misdemeanor, string? disciplinaryResponsibilityKind, int personalizedAccountingCardId)
    {
        public int Id { get; set; } = id;

        public DateOnly? Date { get; set; } = date;

        public string? Misdemeanor { get; set; } = misdemeanor;
        public string? DisciplinaryResponsibilityKind { get; set; } = disciplinaryResponsibilityKind;

        public int PersonalizedAccountingCardId { get; set; } = personalizedAccountingCardId;
    }
}
