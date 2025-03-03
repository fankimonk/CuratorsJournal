using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.GroupActives
{
    public record UpdateGroupActiveRequest
    (
        [Required]
        string PositionName,

        [Required]
        int StudentId
    );
}
