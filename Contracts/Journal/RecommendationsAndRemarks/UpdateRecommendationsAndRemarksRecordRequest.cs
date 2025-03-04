using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.RecommendationsAndRemarks
{
    public record UpdateRecommendationsAndRemarksRecordRequest
    (
        [Required]
        DateOnly Date,
        [Required]
        DateOnly ExecutionDate,

        [Required]
        string Content,
        [Required]
        string Result,

        [Required]
        int ReviewerId
    );
}
