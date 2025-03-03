using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.StudentHealthCards
{
    public record CreateHealthCardRecordRequest
    (
        [Required]
        int Number,
        [Required]
        int MissedClasses,

        string? Note,

        [Required]
        int StudentId,
        [Required]
        int PageId
    );
}
