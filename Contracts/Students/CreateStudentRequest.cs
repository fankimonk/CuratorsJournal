using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Students
{
    public class CreateStudentRequest(
        string firstName, string middleName, string lastName, string? phoneNumber, int? groupId)
    {
        [Required(ErrorMessage = "Поле \"Имя\" обязательно")]
        public string FirstName { get; set; } = firstName;
        [Required(ErrorMessage = "Поле \"Отчество\" обязательно")]
        public string MiddleName { get; set; } = middleName;
        [Required(ErrorMessage = "Поле \"Фамилия\" обязательно")]
        public string LastName { get; set; } = lastName;

        [Phone(ErrorMessage = "Неверный формат номера телефона")]
        [MinLength(7)]
        [MaxLength(19)]
        public string? PhoneNumber { get; set; } = phoneNumber;

        [Required(ErrorMessage = "Поле \"Группа\" обязательно")]
        [NotNull]
        public int? GroupId { get; set; } = groupId;
    }
}
