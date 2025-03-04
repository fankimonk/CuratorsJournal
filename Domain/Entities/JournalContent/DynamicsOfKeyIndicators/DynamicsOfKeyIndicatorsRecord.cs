using Domain.Entities.JournalContent.Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent.DynamicsOfKeyIndicators
{
    [Table("DynamicsOfKeyIndicators")]
    public class DynamicsOfKeyIndicatorsRecord
    {
        public int Id { get; set; }

        public int KeyIndicatorId { get; set; }
        public KeyIndicator? KeyIndicator { get; set; }

        public string? Note { get; set; }

        public int PageId { get; set; }
        public Page? Page;

        public List<KeyIndicatorByCourse> KeyIndicatorsByCourse { get; set; } = [];
    }
}
