using System.ComponentModel.DataAnnotations;

namespace Contracts.Group
{
    public record AppointCuratorRequest
    (
        [Required]
        int GroupId,

        [Required]
        int CuratorId
    );
}
