using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public record UpdateIdeologicalEducationalWorkRecordRequest
    (
        DateOnly? StartDate,

        DateOnly? EndDate,

        string? WorkContent
    );
}
