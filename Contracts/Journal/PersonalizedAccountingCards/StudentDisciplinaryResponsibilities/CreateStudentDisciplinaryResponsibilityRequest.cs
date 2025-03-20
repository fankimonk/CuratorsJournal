using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.PersonalizedAccountingCards.StudentDisciplinaryResponsibilities
{
    public class CreateStudentDisciplinaryResponsibilityRequest(
        DateOnly? date, string? misdemeanor, string? disciplinaryResponsibilityKind, int personalizedAccountingCardId)
    {
        public DateOnly? Date { get; set; } = date;

        public string? Misdemeanor { get; set; } = misdemeanor;
        public string? DisciplinaryResponsibilityKind { get; set; } = disciplinaryResponsibilityKind;

        [Required]
        public int PersonalizedAccountingCardId { get; set; } = personalizedAccountingCardId;
    }
}
