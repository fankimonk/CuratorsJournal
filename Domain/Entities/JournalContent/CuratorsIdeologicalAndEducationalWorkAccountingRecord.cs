using Domain.Entities.JournalContent.Pages;

namespace Domain.Entities.JournalContent
{
    public class CuratorsIdeologicalAndEducationalWorkAccountingRecord
    {
        public int Id { get; set; }

        public int? StartDay { get; set; }
        public int? EndDay { get; set; }

        public string? WorkContent { get; set; } = string.Empty;

        public int PageId { get; set; }
        public Page? Page;
    }
}
