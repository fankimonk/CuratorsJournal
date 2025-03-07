namespace Contracts.Journal.InformationHoursAccounting
{
    public class InformationHoursAccountingPageResponse(int pageId, List<InformationHoursAccountingRecordResponse> informationHoursAccounting)
    {
        public int PageId { get; set; } = pageId;

        public List<InformationHoursAccountingRecordResponse> InformationHoursAccounting { get; set; } = informationHoursAccounting;
    }
}
