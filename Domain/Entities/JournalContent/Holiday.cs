using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.JournalContent
{
    public class Holiday
    {
        public int Id { get; set; }

        [Range(0, 31)]
        public int? Day { get; set; }

        [Range(0, 12)]
        public int? Month { get; set; }

        public string? RelativeDate { get; set; }

        public string Name { get; set; } = string.Empty;

        public int TypeId { get; set; }
        public HolidayType? Type { get; set; }
    }
}
