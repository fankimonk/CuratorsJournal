using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public record CreateIdeologicalEducationalWorkRecordRequest
    (
        [Required]
        DateOnly StartDate,

        [Required]
        DateOnly EndDate,

        [Required]
        string WorkContent,

        [Required]
        int PageId
    );
}
