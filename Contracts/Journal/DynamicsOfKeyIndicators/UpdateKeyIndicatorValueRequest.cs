using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.DynamicsOfKeyIndicators
{
    public record UpdateKeyIndicatorValueRequest
    (
        [Required]
        int Course,

        [Required]
        double Value
    );
}
