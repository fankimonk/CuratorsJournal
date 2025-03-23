using Contracts.CertificationTypes;

namespace Contracts.Journal.FinalPerformanceAccounting
{
    public class FinalPerformanceAccountingPageResponse(int pageId, List<PerformanceAccountingRecordResponse> records,
        List<CertificationTypeResponse> certificationTypes)
    {
        public int PageId { get; set; } = pageId;

        public List<PerformanceAccountingRecordResponse> Records { get; set; } = records;

        public List<CertificationTypeResponse> CertificationTypes { get; set; } = certificationTypes;
    }
}
