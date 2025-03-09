namespace Contracts.Journal.StudentHealthCards
{
    public class HealthCardPageResponse(int pageId,
        List<HealthCardRecordResponse> studentHealthCard,
        HealthCardPageAttributesResponse attributes)
    {
        public int PageId { get; set; } = pageId;

        public List<HealthCardRecordResponse> StudentHealthCard { get; set; } = studentHealthCard;

        public HealthCardPageAttributesResponse Attributes { get; set; } = attributes;
    }
}
