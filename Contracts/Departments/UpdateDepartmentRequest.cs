using System.ComponentModel.DataAnnotations;

namespace Contracts.Departments
{
    public class UpdateDepartmentRequest(
        string name, string abbreviatedName, int headId, int deaneryId)
    {
        [Required]
        public string Name { get; set; } = name;
        [Required]
        public string AbbreviatedName { get; set; } = abbreviatedName;

        [Required]
        public int HeadId { get; set; } = headId;
        [Required]
        public int DeaneryId { get; set; } = deaneryId;
    }
}
