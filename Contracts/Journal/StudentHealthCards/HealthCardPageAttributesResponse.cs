namespace Contracts.Journal.StudentHealthCards
{
    public class HealthCardPageAttributesResponse(int id, int? academicYearId)
    {
        public int Id { get; set; } = id;

        public int? AcademicYearId { get; set; } = academicYearId;
    }
}
