using Domain.Entities.JournalContent.Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    [Table("StudentsHealthCard")]
    public class StudentsHealthCardRecord
    {
        public int Id { get; set; }

        public int? Number { get; set; }
        public int? MissedClasses { get; set; }

        public string? Note { get; set; } = string.Empty;

        public int? StudentId { get; set; }
        public Student? Student { get; set; }

        public int PageId { get; set; }
        public Page? Page;
    }
}
