using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Groups
{
    public class CreateGroupRequest
    (
        string number,
        int? admissionYear,
        int? specialtyId,
        int? curatorId
    )
    {
        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        public string Number { get; set; } = number;

        [Required]
        [Range(0, int.MaxValue)]
        [NotNull]
        public int? AdmissionYear { get; set; } = admissionYear;

        [Required]
        [NotNull]
        public int? SpecialtyId { get; set; } = specialtyId;

        public int? CuratorId { get; set; } = curatorId;
    }
}
