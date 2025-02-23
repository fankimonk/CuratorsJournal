namespace API.Contracts.Journal
{
    public record JournalResponse
    (
        int JournalId,
        string GroupNumber,
        (string, string, string)? CuratorFIO
    );
}
