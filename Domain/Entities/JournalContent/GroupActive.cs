using Domain.Entities.JournalContent.Pages;

namespace Domain.Entities.JournalContent
{
    public class GroupActive
    {
        public int Id { get; set; }

        public string PositionName { get; set; } = string.Empty;
        
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int PageId { get; set; }
        public Page? Page;
    }
}
