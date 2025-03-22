namespace Contracts.Journal.Holidays
{
    public class HolidayResponse(int id, int? day, int? month, string? relativeDate, string? name)
    {
        public int Id { get; set; } = id;
        public int? Day { get; set; } = day;
        public int? Month { get; set; } = month;
        public string? RelativeDate { get; set; } = relativeDate;
        public string? Name { get; set; } = name;
    }
}
