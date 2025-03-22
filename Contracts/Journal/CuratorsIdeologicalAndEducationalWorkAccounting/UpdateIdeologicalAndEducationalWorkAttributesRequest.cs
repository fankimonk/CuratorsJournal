using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public record UpdateIdeologicalAndEducationalWorkAttributesRequest
    (
        [Range(1, 12)]
        int? Month,

        [Range(0, int.MaxValue)]
        int? Year
    );
}
