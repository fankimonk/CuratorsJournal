using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.LiteratureWork
{
    public record CreateLiteratureWorkRecordRequest
    (
        [Required]
        int LiteratureId,

        string? ShortAnnotaion,

        [Required]
        int PageId
    );
}
