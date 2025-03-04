namespace Contracts.Journal.DynamicsOfKeyIndicators
{
    public class DynamicsOfKeyIndicatorsPageResponse(int pageId, List<DynamicsOfKeyIndicatorsRecordResponse> dynamicsOfKeyIndicators)
    {
        public int PageId { get; set; } = pageId;

        public List<DynamicsOfKeyIndicatorsRecordResponse> DynamicsOfKeyIndicators { get; set; } = dynamicsOfKeyIndicators;
    }
}
