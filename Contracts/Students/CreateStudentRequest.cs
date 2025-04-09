using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Students
{
    public class CreateStudentRequest(
        string firstName, string middleName, string lastName, string? phoneNumber, int? groupId)
    {
        [Required]
        public string FirstName { get; set; } = firstName;
        [Required]
        public string MiddleName { get; set; } = middleName;
        [Required]
        public string LastName { get; set; } = lastName;

        [Phone]
        [MinLength(7)]
        [MaxLength(19)]
        public string? PhoneNumber { get; set; } = phoneNumber;

        [Required]
        [NotNull]
        public int? GroupId { get; set; } = groupId;
    }
}
