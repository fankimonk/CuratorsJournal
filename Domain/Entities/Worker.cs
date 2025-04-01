using Domain.Entities.JournalContent;

namespace Domain.Entities
{
    public class Worker
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public int PositionId { get; set; }
        public Position? Position { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public Teacher? Teacher { get; set; }

        public Deanery? DeaneryAsDean { get; set; }
        public Deanery? DeaneryAsDeputyDean { get; set; }

        public Department? Department { get; set; }

        public List<PsychologicalAndPedagogicalCharacteristics> PsychologicalAndPedagogicalCharacteristics { get; set; } = [];
        public List<RecomendationsAndRemarksRecord> RecomendationsAndRemarks { get; set; } = [];
    }
}
