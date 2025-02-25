using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.ContactPhones
{
    public record UpdateContactPhoneRequest
    (
        [Required]
        string Name,

        [Required]
        [Phone]
        [MinLength(9)]
        [MaxLength(17)]
        string PhoneNumber
    );
}
