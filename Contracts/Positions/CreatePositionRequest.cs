using System.ComponentModel.DataAnnotations;

namespace Contracts.Positions
{
    public class CreatePositionRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
