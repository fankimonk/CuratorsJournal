using System.ComponentModel.DataAnnotations;

namespace Contracts.Faculties
{
    public class CreateFacultyRequest(
        string name, string abbreviatedName)
    {
        [Required]
        public string Name { get; set; } = name;

        [Required]
        public string AbbreviatedName { get; set; } = abbreviatedName;
    }
}
