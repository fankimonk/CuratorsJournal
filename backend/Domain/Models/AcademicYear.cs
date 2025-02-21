using Domain.Models.JournalContent;

namespace Domain.Models
{
    public class AcademicYear
    {
        public int Id { get; set; }

        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }

        public List<SocioPedagogicalCharacteristics> SocioPedagogicalCharacteristics { get; set; } = [];
    }
}
