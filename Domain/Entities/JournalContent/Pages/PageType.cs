namespace Domain.Entities.JournalContent.Pages
{
    public class PageType
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? MaxPages { get; set; }

        public List<Page> Pages { get; set; } = [];
    }
}
