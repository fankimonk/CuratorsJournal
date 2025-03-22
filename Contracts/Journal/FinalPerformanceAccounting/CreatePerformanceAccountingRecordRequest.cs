using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.FinalPerformanceAccounting
{
    public record CreatePerformanceAccountingRecordRequest
    (
        int? Number,

        [Required]
        int PageId,

        int? StudentId
    );
}
