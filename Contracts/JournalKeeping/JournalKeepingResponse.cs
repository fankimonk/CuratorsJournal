namespace Contracts.JournalKeeping
{
    public class JournalKeepingResponse(string title, string content)
    {
        public string Title { get; set; } = title;
        public string Content { get; set; } = content;
    }

    public record UpdateJournalKeepingRequest
    (
        string Title,
        string Content
    );
}
