namespace Domain.Entities.JournalContent
{
    public class HolidayType
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Holiday> Holidays { get; set; } = [];
    }
}
