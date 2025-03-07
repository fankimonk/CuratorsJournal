namespace Contracts.Journal.PersonalizedAccountingCards.StudentEncouragements
{
    public record UpdateStudentEncouragementRequest
    (
        DateOnly? Date,

        string? Achievement,
        string? EncouragementKind
    );
}
