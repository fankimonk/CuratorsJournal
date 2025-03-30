using System.ComponentModel.DataAnnotations;

namespace Contracts.Students.PEGroups
{
    public record DeleteStudentPEGroupQuery
    (
        [Required]
        int StudentId,

        [Required]
        int PEGroupId
    );
}
