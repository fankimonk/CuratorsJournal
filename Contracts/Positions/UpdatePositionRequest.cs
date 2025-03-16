using System.ComponentModel.DataAnnotations;

namespace Contracts.Positions
{
    public class UpdatePositionRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
