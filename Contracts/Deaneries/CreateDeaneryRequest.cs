using System.ComponentModel.DataAnnotations;

namespace Contracts.Deaneries
{
    public class CreateDeaneryRequest(
        int facultyId, int deanId, int deputyDeanId)
    {
        [Required]
        public int FacultyId { get; set; } = facultyId;
        [Required]
        public int DeanId { get; set; } = deanId;
        [Required]
        public int DeputyDeanId { get; set; } = deputyDeanId;
    }
}
