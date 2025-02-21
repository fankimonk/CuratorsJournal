namespace Domain.Models
{
    public class Curator
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public List<Group> Groups { get; set; } = [];
    }
}
