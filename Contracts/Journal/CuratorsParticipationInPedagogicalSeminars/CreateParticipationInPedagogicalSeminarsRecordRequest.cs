using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsParticipationInPedagogicalSeminars
{
    public record CreateParticipationInPedagogicalSeminarsRecordRequest
    (
        DateOnly? Date,

        string? Topic,
        string? ParticipationForm,
        string? SeminarLocation,

        string? Note,

        [Required]
        int PageId
    );
}
