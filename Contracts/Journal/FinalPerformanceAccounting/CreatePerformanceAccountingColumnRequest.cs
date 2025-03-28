using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Journal.FinalPerformanceAccounting
{
    public class CreatePerformanceAccountingColumnRequest(int? certificationTypeId, int? subjectId, int pageId)
    {
        [Required]
        [NotNull]
        public int? CertificationTypeId { get; set; } = certificationTypeId;

        public int? SubjectId { get; set; } = subjectId;

        [Required]
        public int PageId { get; set; } = pageId;
    }
}
