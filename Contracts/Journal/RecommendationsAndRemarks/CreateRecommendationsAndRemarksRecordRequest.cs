using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.RecommendationsAndRemarks
{
    public record CreateRecommendationsAndRemarksRecordRequest
    (
        DateOnly? Date,
        DateOnly? ExecutionDate,

        string? Content,
        string? Result,

        int? ReviewerId,

        [Required]
        int PageId
    );
}
