namespace Contracts.Journal.LiteratureWork
{
    public class LiteratureWorkPageResponse(int pageId, List<LiteratureWorkRecordResponse> literatureWork)
    {
        public int PageId { get; set; } = pageId;

        public List<LiteratureWorkRecordResponse> LiteratureWork { get; set; } = literatureWork;
    }
}
