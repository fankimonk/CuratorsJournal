using System.ComponentModel.DataAnnotations;

namespace Contracts.ChronicDiseases
{
    public class CreateChronicDiseaseRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
