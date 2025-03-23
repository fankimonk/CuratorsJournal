using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.Holidays
{
    public class CreateHolidayTypeRequest(string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
