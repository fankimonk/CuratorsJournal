using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Teachers
{
    public class CreateTeacherRequest(string firstName, string middleName, string lastName, int? departmentId)
    {
        [Required]
        public string FirstName { get; set; } = firstName;
        [Required]
        public string MiddleName { get; set; } = middleName;
        [Required]
        public string LastName { get; set; } = lastName;

        [Required]
        [NotNull]
        public int? DepartmentId { get; set; } = departmentId;
    }
}
