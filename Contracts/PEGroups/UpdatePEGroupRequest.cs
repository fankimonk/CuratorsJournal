using System.ComponentModel.DataAnnotations;

namespace Contracts.PEGroups
{
    public class UpdatePEGroupRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
