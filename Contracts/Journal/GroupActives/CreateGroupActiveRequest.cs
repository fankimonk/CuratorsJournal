using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.GroupActives
{
    public record CreateGroupActiveRequest
    (
        [Required]
        string PositionName,

        [Required]
        int StudentId,

        [Required]
        int PageId
    );
}
