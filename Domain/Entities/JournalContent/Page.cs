namespace Domain.Entities.JournalContent
{
    public class Page
    {
        public int Id { get; set; }

        public int PageTypeId { get; set; }
        public PageType? PageType { get; set; }

        public int JournalId { get; set; }
        public Journal? Journal { get; set; }

        public List<ContactPhoneNumber> ContactPhoneNumbers { get; set; } = [];
    }

    public class PageType
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Page> Pages { get; set; } = [];
    }
}
