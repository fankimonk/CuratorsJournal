namespace Domain.Entities.JournalContent.Pages
{
    public class PageType
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Page> Pages { get; set; } = [];
    }
}
