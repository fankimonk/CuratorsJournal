namespace Contracts.Journal.Pages
{
    public record PageResponse
    (
        int Id,
        PageTypeResponse PageType,
        int JournalId
    );
}
