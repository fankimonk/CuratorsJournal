using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    [Table("StudentsHealthCard")]
    public class StudentsHealthCardRecord
    {
        public int Id { get; set; }

        [Range(0, int.MaxValue)]
        public int Number { get; set; }

        [Range(0, int.MaxValue)]
        public int MissedClasses { get; set; }

        public string? Note { get; set; } = string.Empty;

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int AcademicYearId { get; set; }
        public AcademicYear? AcademicYear { get; set; }

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
