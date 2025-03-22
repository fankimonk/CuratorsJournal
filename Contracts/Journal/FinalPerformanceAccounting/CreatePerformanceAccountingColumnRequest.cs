using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.FinalPerformanceAccounting
{
    public record CreatePerformanceAccountingColumnRequest
    (
        [Required]
        int CertificationTypeId,

        int? SubjectId,

        [Required]
        int PageId
    );
}
