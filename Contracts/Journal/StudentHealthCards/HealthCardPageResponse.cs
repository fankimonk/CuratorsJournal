namespace Contracts.Journal.StudentHealthCards
{
    public class HealthCardPageResponse(int pageId, List<HealthCardRecordResponse> studentHealthCard)
    {
        public int PageId { get; set; } = pageId;

        public List<HealthCardRecordResponse> StudentHealthCard { get; set; } = studentHealthCard;
    }
}
