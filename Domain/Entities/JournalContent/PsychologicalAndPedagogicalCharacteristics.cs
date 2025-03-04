using Domain.Entities.JournalContent.Pages;

namespace Domain.Entities.JournalContent
{
    public class PsychologicalAndPedagogicalCharacteristics
    {
        public int Id { get; set; }

        public string? Content { get; set; } = string.Empty;

        public DateOnly? Date { get; set; }

        public int? WorkerId { get; set; }
        public Worker? Worker { get; set; }

        public int PageId { get; set; }
        public Page? Page;
    }
}
