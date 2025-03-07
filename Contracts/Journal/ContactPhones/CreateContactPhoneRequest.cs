using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.ContactPhones
{
    public class CreateContactPhoneRequest(
        string? name, string? phoneNumber, int pageId)
    {
        public string? Name { get; set; } = name;

        [Phone]
        [MinLength(7)]
        [MaxLength(19)]
        public string? PhoneNumber { get; set; } = phoneNumber;

        [Required]
        public int PageId { get; set; } = pageId;
    };
}
