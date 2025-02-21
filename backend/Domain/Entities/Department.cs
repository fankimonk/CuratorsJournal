namespace Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int HeadId { get; set; }
        public HeadOfDepartment? HeadOfDepartment { get; set; }

        public int DeaneryId { get; set; }
        public Deanery? Deanery { get; set; }

        public List<Teacher> Teachers { get; set; } = [];
        public List<Specialty> Specialties { get; set; } = [];
    }
}
