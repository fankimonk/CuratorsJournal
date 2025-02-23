namespace Domain.Entities.JournalContent
{
    public class RecomendationsAndRemarks
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }
        public DateOnly ExecutionDate { get; set; } 

        public string Content { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;

        public int ReviewerId { get; set; }
        public Worker? Reviewer { get; set; }

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
