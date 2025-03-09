using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.GroupActives
{
    public record CreateGroupActiveRequest
    (
        string? PositionName,

        int? StudentId,

        [Required]
        int PageId
    );
}
