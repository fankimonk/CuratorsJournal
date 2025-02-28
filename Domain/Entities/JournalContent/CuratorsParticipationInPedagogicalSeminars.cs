namespace Domain.Entities.JournalContent
{
    public class CuratorsParticipationInPedagogicalSeminars
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public string Topic { get; set; } = string.Empty;
        public string ParticipationForm { get; set; } = string.Empty;
        public string SeminarLocation { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;

        public int PageId { get; set; }
        public Page? Page;
    }
}
