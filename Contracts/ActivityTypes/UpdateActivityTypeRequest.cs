using System.ComponentModel.DataAnnotations;

namespace Contracts.ActivityTypes
{
    public class UpdateActivityTypeRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
