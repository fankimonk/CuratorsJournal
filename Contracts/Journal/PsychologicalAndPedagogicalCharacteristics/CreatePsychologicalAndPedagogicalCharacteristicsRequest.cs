using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.PsychologicalAndPedagogicalCharacteristics
{
    public record CreatePsychologicalAndPedagogicalCharacteristicsRequest
    (
        string? Content,

        DateOnly? Date,

        int? WorkerId,

        [Required]
        int PageId
    );
}
