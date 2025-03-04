using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.Traditions
{
    public record UpdateTraditionRequest
    (
        [Required]
        string Name,
        [Required]
        string ParticipationForm,

        string? Note,

        [Required]
        [Range(1, 31)]
        int Day,
        [Required]
        [Range(1, 12)]
        int Month
    );
}
