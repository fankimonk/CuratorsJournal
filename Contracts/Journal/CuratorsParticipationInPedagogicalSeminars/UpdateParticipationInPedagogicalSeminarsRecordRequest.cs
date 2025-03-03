using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsParticipationInPedagogicalSeminars
{
    public record UpdateParticipationInPedagogicalSeminarsRecordRequest
    (
        [Required]
        DateOnly Date,

        [Required]
        string Topic,
        [Required]
        string ParticipationForm,
        [Required]
        string SeminarLocation,

        string? Note
    );
}
