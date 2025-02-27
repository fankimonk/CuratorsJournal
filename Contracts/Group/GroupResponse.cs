namespace Contracts.Group
{
    public class GroupResponse(int id, string number, int admissionYear, int specialtyId)
    {
        public int Id { get; set; } = id;
        public string Number { get; set; } = number;
        public int AdmissionYear { get; set; } = admissionYear;
        public int SpecialtyId { get; set; } = specialtyId;
    }
}
