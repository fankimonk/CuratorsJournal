namespace Domain.Entities.JournalContent
{
    public class Tradition
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string ParticipationForm { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
