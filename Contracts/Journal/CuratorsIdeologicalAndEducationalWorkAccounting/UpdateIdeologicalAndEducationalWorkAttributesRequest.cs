using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public record UpdateIdeologicalAndEducationalWorkAttributesRequest
    (
        [Range(1, 12)]
        int? Month,

        [Range(0, 99, ErrorMessage = "Год должен быть от 0 до 99")]
        int? Year
    );
}
