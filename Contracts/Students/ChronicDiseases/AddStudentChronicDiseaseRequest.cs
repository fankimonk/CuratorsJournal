using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Students.ChronicDiseases
{
    public class AddStudentChronicDiseaseRequest(int studentId, int? chronicDiseaseId)
    {
        [Required]
        public int StudentId { get; set; } = studentId;

        [Required]
        [NotNull]
        public int? ChronicDiseaseId { get; set; } = chronicDiseaseId;
    }
}
