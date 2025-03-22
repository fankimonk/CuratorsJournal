namespace Contracts.Journal.FinalPerformanceAccounting
{
    public class FinalPerformanceAccountingPageResponse(int pageId, List<PerformanceAccountingRecordResponse> records,
        List<PerformanceAccountingColumnResponse> columns)
    {
        public int PageId { get; set; } = pageId;

        public List<PerformanceAccountingRecordResponse> Records { get; set; } = records;

        public List<PerformanceAccountingColumnResponse> Columns { get; set; } = columns;
    }
}
