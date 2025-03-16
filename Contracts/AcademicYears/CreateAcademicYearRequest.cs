using System.ComponentModel.DataAnnotations;

namespace Contracts.AcademicYears
{
    public class CreateAcademicYearRequest(
        int startYear, int endYear)
    {
        [Required]
        public int StartYear { get; set; } = startYear;

        [Required]
        public int EndYear { get; set; } = endYear;
    }
}
