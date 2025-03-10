using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.RecommendationsAndRemarks
{
    public record UpdateRecommendationsAndRemarksRecordRequest
    (
        DateOnly? Date,
        DateOnly? ExecutionDate,

        string? Content,
        string? Result,

        int? ReviewerId
    );
}
