using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.ContactPhones
{
    public record UpdateContactPhoneRequest
    (
        string? Name,

        [Phone]
        [MinLength(7)]
        [MaxLength(19)]
        string? PhoneNumber
    );
}
