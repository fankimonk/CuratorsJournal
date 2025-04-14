namespace Contracts.Journal.Pages
{
    public class PageResponse
    (
        int id,
        int journalId,
        bool isApproved,
        PageTypeResponse? pageType
    )
    {
        public int Id { get; set; } = id;
        public int JournalId { get; set; } = journalId;
        public bool IsApproved { get; set; } = isApproved;
        public PageTypeResponse? PageType { get; set; } = pageType;
    }
}
