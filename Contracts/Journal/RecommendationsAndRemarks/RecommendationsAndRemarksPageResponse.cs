namespace Contracts.Journal.RecommendationsAndRemarks
{
    public class RecommendationsAndRemarksPageResponse(int pageId, 
        List<RecommendationsAndRemarksRecordResponse> recommendationsAndRemarks)
    {
        public int PageId { get; set; } = pageId;

        public List<RecommendationsAndRemarksRecordResponse> RecommendationsAndRemarks { get; set; }
            = recommendationsAndRemarks;
    }
}
