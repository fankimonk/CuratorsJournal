namespace Contracts.Journal.PersonalizedAccountingCards.WorkWithParents
{
    public record UpdateWorkWithParentsRecordRequest
    (
        DateOnly? Date,

        string? WorkContent,
        string? Note
    );
}
