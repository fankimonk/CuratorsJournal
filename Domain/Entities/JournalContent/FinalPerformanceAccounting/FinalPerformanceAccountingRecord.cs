using Domain.Entities.JournalContent.Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent.FinalPerformanceAccounting
{
    [Table("FinalPerformanceAccounting")]
    public class FinalPerformanceAccountingRecord
    {
        public int Id { get; set; }

        public int? Number { get; set; }

        public int PageId { get; set; }
        public Page? Page;

        public int? StudentId { get; set; }
        public Student? Student { get; set; }

        public List<PerformanceAccountingGrade> PerformanceAccountingGrades { get; set; } = [];
    }

}
