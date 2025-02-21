using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    //TODO
    [Table("FinalPerformanceAccounting")]
    public class FinalPerformanceAccountingRecord
    {
        public int Id { get; set; }

        [Range(0, int.MaxValue)]
        public int Number { get; set; }



        public int JournalId { get; set; }
        public Journal? Journal;

        public int StudentId { get; set; }
        public Student? Student { get; set; }
    }
}
