using System.ComponentModel.DataAnnotations;

namespace Contracts.Specialties
{
    public class UpdateSpecialtyRequest(
        string name, string abbreviatedName, int departmentId)
    {
        [Required]
        public string Name { get; set; } = name;
        [Required]
        public string AbbreviatedName { get; set; } = abbreviatedName;

        [Required]
        public int DepartmentId { get; set; } = departmentId;
    }
}
