using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.Traditions
{
    public class CreateTraditionRequest(
        string name, string participationForm, string? note, int day, int month, int pageId)
    {
        [Required]
        public string Name { get; set; } = name;
        [Required]
        public string ParticipationForm { get; set; } = participationForm;

        public string? Note { get; set; } = note;

        [Required]
        [Range(1, 31)]
        public int Day { get; set; } = day;
        [Required]
        [Range(1, 12)]
        public int Month { get; set; } = month;

        [Required]
        public int PageId { get; set; } = pageId;
    };
}
