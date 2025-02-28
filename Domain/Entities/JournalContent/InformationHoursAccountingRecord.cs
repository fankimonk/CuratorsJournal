using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    //TODO
    [Table("InformationHoursAccounting")]
    public class InformationHoursAccountingRecord
    {
        public int Id { get; set; }



        public int PageId { get; set; }
        public Page? Page;
    }
}
