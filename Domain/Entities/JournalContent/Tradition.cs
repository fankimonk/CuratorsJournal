using Domain.Entities.JournalContent.Pages;

namespace Domain.Entities.JournalContent
{
    public class Tradition
    {
        public int Id { get; set; }

        public string? Name { get; set; } = string.Empty;
        public string? ParticipationForm { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;

        public int? Day { get; set; }
        public int? Month { get; set; }

        public int PageId { get; set; }
        public Page? Page;
    }
}
