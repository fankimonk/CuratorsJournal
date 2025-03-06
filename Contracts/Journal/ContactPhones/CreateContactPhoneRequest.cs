using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.ContactPhones
{
    public class CreateContactPhoneRequest(
        string name, string phoneNumber, int pageId)
    {
        [Required]
        public string Name { get; set; } = name;

        [Required]
        [Phone]
        [MinLength(9)]
        [MaxLength(17)]
        public string PhoneNumber { get; set; } = phoneNumber;

        [Required]
        public int PageId { get; set; } = pageId;
    };
}
