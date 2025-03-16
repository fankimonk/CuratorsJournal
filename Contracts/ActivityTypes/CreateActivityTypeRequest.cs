using System.ComponentModel.DataAnnotations;

namespace Contracts.ActivityTypes
{
    public class CreateActivityTypeRequest(
        string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
