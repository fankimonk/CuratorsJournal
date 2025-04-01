using System.ComponentModel.DataAnnotations;

namespace Contracts.Teachers
{
    public class UpdateTeacherRequest(string firstName, string middleName, string lastName, int departmentId)
    {
        [Required]
        public string FirstName { get; set; } = firstName;
        [Required]
        public string MiddleName { get; set; } = middleName;
        [Required]
        public string LastName { get; set; } = lastName;

        [Required]
        public int DepartmentId { get; set; } = departmentId;
    }
}
