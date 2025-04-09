using System.ComponentModel.DataAnnotations;

namespace Contracts.Students
{
    public class UpdateStudentRequest(
        string firstName, string middleName, string lastName, string? phoneNumber, int groupId)
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
        public int GroupId { get; set; } = groupId;
    }
}
