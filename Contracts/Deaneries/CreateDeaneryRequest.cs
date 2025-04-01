using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Deaneries
{
    public class CreateDeaneryRequest(
        int? facultyId, int? deanId, int? deputyDeanId)
    {
        [Required]
        [NotNull]
        public int? FacultyId { get; set; } = facultyId;
        [Required]
        [NotNull]
        public int? DeanId { get; set; } = deanId;
        [Required]
        [NotNull]
        public int? DeputyDeanId { get; set; } = deputyDeanId;
    }
}
