using Contracts.Journal.Pages;

namespace Contracts.Journal
{
    public record JournalPagesResponse
    (
        int JournalId,
        List<PageResponse> Pages
    );
}
