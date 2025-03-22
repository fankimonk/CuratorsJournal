using System.ComponentModel.DataAnnotations;

namespace Contracts.CertificationTypes
{
    public class CreateCertificationTypeRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
