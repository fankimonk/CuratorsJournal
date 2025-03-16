namespace Contracts.Deaneries
{
    public class DeaneryResponse(
        int id, int facultyId, int deanId, int deputyDeanId)
    {
        public int Id { get; set; } = id;

        public int FacultyId { get; set; } = facultyId;

        public int DeanId { get; set; } = deanId;

        public int DeputyDeanId { get; set; } = deputyDeanId;
    }
}
