using System.ComponentModel.DataAnnotations;

namespace Contracts.ChronicDiseases
{
    public class UpdateChronicDiseaseRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
