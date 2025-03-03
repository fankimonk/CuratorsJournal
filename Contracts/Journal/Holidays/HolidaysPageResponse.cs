namespace Contracts.Journal.Holidays
{
    public class HolidaysPageResponse(List<HolidayTypeResponse> holidayTypes)
    {
        public List<HolidayTypeResponse> HolidayTypes { get; set; } = holidayTypes;
    }
}
