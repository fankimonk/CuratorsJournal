using Contracts.Faculties;

namespace Contracts.Deaneries
{
    public class DeaneryResponse(
        int id, int facultyId, int deanId, int deputyDeanId, FacultyResponse? faculty)
    {
        public int Id { get; set; } = id;

        public int FacultyId { get; set; } = facultyId;

        public int DeanId { get; set; } = deanId;

        public int DeputyDeanId { get; set; } = deputyDeanId;

        public FacultyResponse? Faculty { get; set; } = faculty;
    }
}
