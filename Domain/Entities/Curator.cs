namespace Domain.Entities
{
    public class Curator
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public List<Group> Groups { get; set; } = [];
        public List<CuratorsAppointmentHistoryRecord> CuratorsAppointmentHistory { get; set; } = [];
    }
}
