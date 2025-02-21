namespace Domain.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public int SpecialtyId { get; set; }
        public Specialty? Specialty { get; set; }

        public int CuratorId { get; set; }
        public Curator? Curator { get; set; }

        public List<Student> Students { get; set; } = [];

        public Journal? Journal { get; set; }
    }
}
