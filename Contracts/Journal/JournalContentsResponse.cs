using Contracts.Journal.Pages;

namespace Contracts.Journal
{
    public record JournalContentsResponse
    (
        int JournalId,
        List<PageTypeResponse> PageTypes
    );
}
