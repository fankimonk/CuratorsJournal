namespace Contracts.Journal.Holidays
{
    public class HolidaysPageResponse(int pageId, List<HolidayTypeResponse> holidayTypes)
    {
        public int PageId { get; set; } = pageId;

        public List<HolidayTypeResponse> HolidayTypes { get; set; } = holidayTypes;
    }
}
