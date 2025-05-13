using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public record CreateIdeologicalEducationalWorkRecordRequest
    (
        [Range(0, 31)]
        int? StartDay,
        [Range(0, 31)]
        int? EndDay,

        string? WorkContent,

        [Required]
        int PageId
    );
}
