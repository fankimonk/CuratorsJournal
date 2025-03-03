namespace Contracts.Journal.Holidays
{
    public class HolidaysPageResponse(List<HolidayTypeResponse> holidayTypes)
    {
        public List<HolidayTypeResponse> HolidayTypes { get; set; } = holidayTypes;
    }

    public class HolidayResponse(int id, int? day, int? month, string? relativeDate, string name)
    {
        public int Id { get; set; } = id;
        public int? Day { get; set; } = day;
        public int? Month { get; set; } = month;
        public string? RelativeDate { get; set; } = relativeDate;
        public string Name { get; set; } = name;
    }

    public class HolidayTypeResponse(int id, string name, List<HolidayResponse> holidays)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public List<HolidayResponse> Holidays { get; set; } = holidays;
    }
}
