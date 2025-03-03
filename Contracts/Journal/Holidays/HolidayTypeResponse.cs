namespace Contracts.Journal.Holidays
{
    public class HolidayTypeResponse(int id, string name, List<HolidayResponse> holidays)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public List<HolidayResponse> Holidays { get; set; } = holidays;
    }
}
