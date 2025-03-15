using Domain.Entities.JournalContent.Pages.Attributes;

namespace Domain.Entities
{
    public class AcademicYear
    {
        public int Id { get; set; }

        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public List<SocioPedagogicalCharacteristicsPageAttributes> SocioPedagogicalCharacteristicsPageAttributes { get; set; } = [];
        public List<HealthCardPageAttributes> HealthCardPageAttributes { get; set; } = [];
    }
}
