namespace Domain.Entities.JournalContent.Holidays
{
    public class Holiday
    {
        public int Id { get; set; }

        public int? Day { get; set; }
        public int? Month { get; set; }

        public string? RelativeDate { get; set; }

        public string? Name { get; set; } = string.Empty;

        public int TypeId { get; set; }
        public HolidayType? Type { get; set; }
    }
}
