namespace Contracts.Journal
{
    public record PageResponse
    (
        int Id,
        PageTypeResponse PageType,
        int JournalId
    );

    public record PageTypeResponse
    (
        int Id,
        string Name
    );

    public record ContentsResponse
    (
        int JournalId,
        List<PageResponse> Pages
    );
}
