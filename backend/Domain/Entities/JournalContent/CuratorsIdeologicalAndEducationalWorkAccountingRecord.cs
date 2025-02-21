using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.JournalContent
{
    public class CuratorsIdeologicalAndEducationalWorkAccountingRecord
    {
        public int Id { get; set; }

        [Range(0, 12)]
        public int Month { get; set; }

        [Range(0, int.MaxValue)]
        public int Year { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public string WorkContent { get; set; } = string.Empty;

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
