namespace Contracts.Journal.PersonalizedAccountingCards.StudentDisciplinaryResponsibilities
{
    public record UpdateStudentStudentDisciplinaryResponsibilityRequest
    (
        DateOnly? Date,

        string? Misdemeanor,
        string? DisciplinaryResponsibilityKind
    );
}
