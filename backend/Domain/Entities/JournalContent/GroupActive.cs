namespace Domain.Entities.JournalContent
{
    public class GroupActive
    {
        public int Id { get; set; }

        public string PositionName { get; set; } = string.Empty;
        
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
