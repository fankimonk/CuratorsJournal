namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public class IdeologicalEducationalWorkPageResponse(int pageId, 
        List<IdeologicalEducationalWorkRecordResponse> ideologicalEducationalWork)
    {
        public int PageId { get; set; } = pageId;

        public List<IdeologicalEducationalWorkRecordResponse> IdeologicalEducationalWork { get; set; } = ideologicalEducationalWork;
    }
}
