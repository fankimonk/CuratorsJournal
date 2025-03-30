using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Students.PEGroups
{
    public class AddStudentPEGroupRequest(int studentId, int? peGroupId)
    {
        [Required]
        public int StudentId { get; set; } = studentId;

        [Required]
        [NotNull]
        public int? PEGroupId { get; set; } = peGroupId;
    }
}
