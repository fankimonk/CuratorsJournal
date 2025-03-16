namespace Contracts.Groups
{
    public class GroupResponse(int id, string number, int admissionYear, int specialtyId, int? curatorId)
    {
        public int Id { get; set; } = id;
        public string Number { get; set; } = number;
        public int AdmissionYear { get; set; } = admissionYear;
        public int SpecialtyId { get; set; } = specialtyId;
        public int? CuratorId { get; set; } = curatorId;
    }
}
