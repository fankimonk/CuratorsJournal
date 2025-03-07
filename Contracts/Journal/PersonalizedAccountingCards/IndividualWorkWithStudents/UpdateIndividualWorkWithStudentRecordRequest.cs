namespace Contracts.Journal.PersonalizedAccountingCards.IndividualWorkWithStudents
{
    public record UpdateIndividualWorkWithStudentRecordRequest
    (
        DateOnly? Date,

        string? WorkDoneAndRecommendations,
        string? Result
    );
}
