using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.Holidays
{
    public class UpdateHolidayTypeRequest(string name)
    {
        [Required]
        public string Name { get; set; } = name;
    }
}
