using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.AcademicYears
{
    public class CreateAcademicYearRequest(
        int? startYear)
    {
        [Required]
        [NotNull]
        [Range(0, 98)]
        public int? StartYear { get; set; } = startYear;
    }
}
