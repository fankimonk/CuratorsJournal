using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.ContactPhones
{
    public record UpdateContactPhoneRequest
    (
        string? Name,

        [Phone(ErrorMessage = "Введен невалидный номер")]
        [MinLength(7, ErrorMessage = "Номер должен иметь хотя бы 7 символов")]
        [MaxLength(19, ErrorMessage = "Номер не может превышать 19 символов")]
        string? PhoneNumber
    );
}
