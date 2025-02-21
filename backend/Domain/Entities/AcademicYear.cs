using Domain.Entities.JournalContent;

namespace Domain.Entities
{
    public class AcademicYear
    {
        public int Id { get; set; }

        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }

        public List<SocioPedagogicalCharacteristics> SocioPedagogicalCharacteristics { get; set; } = [];
        public List<StudentsHealthCardRecord> StudentsHealthCards { get; set; } = [];
    }
}
