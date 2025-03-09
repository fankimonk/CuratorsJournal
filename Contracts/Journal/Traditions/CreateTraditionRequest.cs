using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.Traditions
{
    public class CreateTraditionRequest(
        string? name, string? participationForm, string? note, int? day, int? month, int pageId)
    {
        public string? Name { get; set; } = name;
        public string? ParticipationForm { get; set; } = participationForm;

        public string? Note { get; set; } = note;

        [Range(1, 31)]
        public int? Day { get; set; } = day;
        [Range(1, 12)]
        public int? Month { get; set; } = month;

        [Required]
        public int PageId { get; set; } = pageId;
    };
}
