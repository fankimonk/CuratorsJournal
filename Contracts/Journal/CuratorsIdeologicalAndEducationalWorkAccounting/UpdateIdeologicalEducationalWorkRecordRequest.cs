using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public record UpdateIdeologicalEducationalWorkRecordRequest
    (
        [Required]
        DateOnly StartDate,

        [Required]
        DateOnly EndDate,

        [Required]
        string WorkContent
    );
}
