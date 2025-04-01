using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Departments
{
    public class CreateDepartmentRequest(
        string name, string abbreviatedName, int? headId, int? deaneryId)
    {
        [Required]
        public string Name { get; set; } = name;
        [Required]
        public string AbbreviatedName { get; set; } = abbreviatedName;

        [Required]
        [NotNull]
        public int? HeadId { get; set; } = headId;
        [Required]
        [NotNull]
        public int? DeaneryId { get; set; } = deaneryId;
    }
}
