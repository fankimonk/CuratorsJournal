using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.AcademicYears
{
    public class CreateAcademicYearRequest(
        int? startYear)
    {
        [Required]
        [NotNull]
        public int? StartYear { get; set; } = startYear;
    }
}
