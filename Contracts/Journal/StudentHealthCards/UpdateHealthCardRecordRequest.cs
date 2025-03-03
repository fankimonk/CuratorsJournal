using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.StudentHealthCards
{
    public record UpdateHealthCardRecordRequest
    (
        [Required]
        int Number,
        [Required]
        int MissedClasses,

        string? Note
    );
}
