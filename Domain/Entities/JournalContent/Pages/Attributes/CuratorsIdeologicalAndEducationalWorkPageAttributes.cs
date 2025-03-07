namespace Domain.Entities.JournalContent.Pages.Attributes
{
    public class CuratorsIdeologicalAndEducationalWorkPageAttributes
    {
        public int Id { get; set; }

        public int PageId { get; set; }
        public Page? Page { get; set; }

        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
