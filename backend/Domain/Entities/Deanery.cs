namespace Domain.Entities
{
    public class Deanery
    {
        public int Id { get; set; }

        public int FacultyId { get; set; }
        public Faculty? Faculty { get; set; }

        public int DeanId { get; set; }
        public Dean? Dean { get; set; }

        public int DeputyDeanId { get; set; }
        public DeputyDean? DeputyDean { get; set; }

        public List<Department> Departments { get; set; } = [];
    }
}
