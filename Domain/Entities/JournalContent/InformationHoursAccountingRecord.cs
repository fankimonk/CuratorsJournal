using Domain.Entities.JournalContent.Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    [Table("InformationHoursAccounting")]
    public class InformationHoursAccountingRecord
    {
        public int Id { get; set; }

        public DateOnly? Date { get; set; }

        public string? Topic { get; set; }
        public string? Note { get; set; }

        public int PageId { get; set; }
        public Page? Page;
    }
}
