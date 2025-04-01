namespace Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public List<Group> Groups { get; set; } = [];
        public List<CuratorsAppointmentHistoryRecord> CuratorsAppointmentHistory { get; set; } = [];
    }
}
