namespace Application.Entities
{
    public class JournalKeeping(string title, string content)
    {
        public string Title { get; set; } = title;
        public string Content { get; set; } = content;
    }
}
