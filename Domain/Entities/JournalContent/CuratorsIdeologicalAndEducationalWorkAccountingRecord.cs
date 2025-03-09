using Domain.Entities.JournalContent.Pages;

namespace Domain.Entities.JournalContent
{
    public class CuratorsIdeologicalAndEducationalWorkAccountingRecord
    {
        public int Id { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public string? WorkContent { get; set; } = string.Empty;

        public int PageId { get; set; }
        public Page? Page;
    }
}
