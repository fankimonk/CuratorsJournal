using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.DynamicsOfKeyIndicators
{
    public record UpdateKeyIndicatorValueRequest
    (
        [Range(0, int.MaxValue)]
        double? Value
    );
}
