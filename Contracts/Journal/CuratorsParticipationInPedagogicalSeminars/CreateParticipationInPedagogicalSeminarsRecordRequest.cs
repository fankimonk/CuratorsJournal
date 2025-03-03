using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsParticipationInPedagogicalSeminars
{
    public record CreateParticipationInPedagogicalSeminarsRecordRequest
    (
        [Required]
        DateOnly Date,

        [Required]
        string Topic,
        [Required]
        string ParticipationForm,
        [Required]
        string SeminarLocation,

        string? Note,

        [Required]
        int PageId
    );
}
