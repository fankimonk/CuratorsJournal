using System.ComponentModel.DataAnnotations;

namespace Contracts.Group
{
    public record CreateGroupRequest
    (
        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        string Number,

        [Required]
        int AdmissionYear,

        [Required]
        int SpecialtyId
    );
}
