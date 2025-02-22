using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }

        [MinLength(8)]
        [MaxLength(8)]
        public string Number { get; set; } = string.Empty;

        public int AdmissionYear { get; set; }

        public int SpecialtyId { get; set; }
        public Specialty? Specialty { get; set; }

        public int? CuratorId { get; set; }
        public Curator? Curator { get; set; }

        public List<Student> Students { get; set; } = [];

        public Journal? Journal { get; set; }
    }
}
