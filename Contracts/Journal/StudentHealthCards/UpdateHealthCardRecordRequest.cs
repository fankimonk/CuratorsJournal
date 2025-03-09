namespace Contracts.Journal.StudentHealthCards
{
    public record UpdateHealthCardRecordRequest
    (
        int? Number,
        int? MissedClasses,

        string? Note,
        int? StudentId
    );
}
