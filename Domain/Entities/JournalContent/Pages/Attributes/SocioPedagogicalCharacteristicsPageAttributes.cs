namespace Domain.Entities.JournalContent.Pages.Attributes
{
    public class SocioPedagogicalCharacteristicsPageAttributes
    {
        public int Id { get; set; }

        public int PageId { get; set; }
        public Page? Page { get; set; }

        public int? AcademicYearId { get; set; }
        public AcademicYear? AcademicYear { get; set; }
    }
}
