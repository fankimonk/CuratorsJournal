using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.DynamicsOfKeyIndicators
{
    public record CreateKeyIndicatorValueRequest
    (
        [Required]
        int DynamicsRecordId,

        [Required]
        int Course,
        [Required]
        double Value
    );
}
