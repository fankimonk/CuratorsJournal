namespace Contracts.Journal.Pages
{
    public record PageResponse
    (
        int Id,
        int JournalId,
        PageTypeResponse? PageType
    );
}
