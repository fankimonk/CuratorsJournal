using System.ComponentModel.DataAnnotations;

namespace Contracts.PEGroups
{
    public class CreatePEGroupRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
