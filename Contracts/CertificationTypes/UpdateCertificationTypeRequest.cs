using System.ComponentModel.DataAnnotations;

namespace Contracts.CertificationTypes
{
    public class UpdateCertificationTypeRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
