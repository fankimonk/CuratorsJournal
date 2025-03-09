using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public record CreateIdeologicalEducationalWorkRecordRequest
    (
        DateOnly? StartDate,

        DateOnly? EndDate,

        string? WorkContent,

        [Required]
        int PageId
    );
}
