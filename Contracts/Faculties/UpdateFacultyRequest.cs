using System.ComponentModel.DataAnnotations;

namespace Contracts.Faculties
{
    public class UpdateFacultyRequest
    (
        string name,
        string abbreviatedName
    )
    {
        [Required]
        public string Name { get; set; } = name;

        [Required]
        public string AbbreviatedName { get; set; } = abbreviatedName;
    }
}
