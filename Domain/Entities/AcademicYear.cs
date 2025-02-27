using Domain.Entities.JournalContent;

namespace Domain.Entities
{
    public class AcademicYear
    {
        public int Id { get; set; }

        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public List<SocioPedagogicalCharacteristics> SocioPedagogicalCharacteristics { get; set; } = [];
        public List<StudentsHealthCardRecord> StudentsHealthCards { get; set; } = [];
    }
}
