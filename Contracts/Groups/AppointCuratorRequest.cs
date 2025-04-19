using System.ComponentModel.DataAnnotations;

namespace Contracts.Groups
{
    public record AppointCuratorRequest
    (
        [Required]
        int GroupId,

        int? CuratorId
    );
}
