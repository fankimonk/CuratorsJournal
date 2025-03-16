using System.ComponentModel.DataAnnotations;

namespace Contracts.Subjects
{
    public class UpdateSubjectRequest(
        string name, string abbreviatedName)
    {
        [Required]
        public string Name { get; set; } = name;

        [Required]
        public string AbbreviatedName { get; set; } = abbreviatedName;
    }
}
