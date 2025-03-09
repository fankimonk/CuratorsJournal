using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.StudentHealthCards
{
    public record CreateHealthCardRecordRequest
    (
        int? Number,
        int? MissedClasses,

        string? Note,

        int? StudentId,

        [Required]
        int PageId
    );
}
