using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.LiteratureWork
{
    public record CreateLiteratureWorkRecordRequest
    (
        int? LiteratureId,

        string? ShortAnnotaion,

        [Required]
        int PageId
    );
}
