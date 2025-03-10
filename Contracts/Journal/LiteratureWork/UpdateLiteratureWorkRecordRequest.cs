namespace Contracts.Journal.LiteratureWork
{
    public record UpdateLiteratureWorkRecordRequest
    (
        int? LiteratureId,

        string? ShortAnnotaion
    );
}
