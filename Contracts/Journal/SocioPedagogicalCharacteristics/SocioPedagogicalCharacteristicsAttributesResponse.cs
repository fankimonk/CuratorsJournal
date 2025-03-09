namespace Contracts.Journal.SocioPedagogicalCharacteristics
{
    public class SocioPedagogicalCharacteristicsAttributesResponse(int id, int? academicYearId)
    {
        public int Id { get; set; } = id;
        public int? AcademicYearId { get; set; } = academicYearId;
    }
}
