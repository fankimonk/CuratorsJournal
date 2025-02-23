using Contracts.Curator;

namespace Contracts.Journal
{
    public record JournalResponse
    (
        int Id,
        string GroupNumber,
        CuratorResponse? Curator
    );
}
