using System.ComponentModel.DataAnnotations;

namespace Contracts.Students.ChronicDiseases
{
    public record DeleteStudentChronicDiseaseQuery
    (
        [Required]
        int StudentId,

        [Required]
        int DiseaseId
    );
}
