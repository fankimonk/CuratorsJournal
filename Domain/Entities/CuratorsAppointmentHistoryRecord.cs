namespace Domain.Entities
{
    public class CuratorsAppointmentHistoryRecord
    {
        public int Id { get; set; }

        public DateOnly AppointmentDate { get; set; }

        public int CuratorId { get; set; }
        public Curator? Curator { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
