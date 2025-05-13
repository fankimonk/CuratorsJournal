using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public record UpdateIdeologicalEducationalWorkRecordRequest
    (
        [Range(0, 31)]
        int? StartDate,
        [Range(0, 31)]
        int? EndDate,

        string? WorkContent
    );
}
