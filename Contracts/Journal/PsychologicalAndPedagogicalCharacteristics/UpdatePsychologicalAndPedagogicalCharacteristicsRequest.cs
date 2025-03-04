using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.PsychologicalAndPedagogicalCharacteristics
{
    public record UpdatePsychologicalAndPedagogicalCharacteristicsRequest
    (
        string? Content,

        DateOnly? Date,

        int? WorkerId
    );
}
