namespace Contracts.Journal.Pages
{
    public record AddPageRequest
    (
        int JournalId,
        int PageTypeId
    );
}
