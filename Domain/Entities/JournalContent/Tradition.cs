using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.JournalContent
{
    public class Tradition
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string ParticipationForm { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;

        [Range(0, 31)]
        public int Day { get; set; }

        [Range(0, 12)]
        public int Month { get; set; }

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
