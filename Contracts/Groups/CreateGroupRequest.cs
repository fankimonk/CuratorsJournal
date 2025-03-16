using System.ComponentModel.DataAnnotations;

namespace Contracts.Groups
{
    public class CreateGroupRequest
    (
        string number,
        int admissionYear,
        int specialtyId,
        int? curatorId
    )
    {
        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        public string Number { get; set; } = number;

        [Required]
        public int AdmissionYear { get; set; } = admissionYear;

        [Required]
        public int SpecialtyId { get; set; } = specialtyId;

        public int? CuratorId { get; set; } = curatorId;
    }
}
