using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    //TODO
    [Table("DynamicsOfKeyIndicators")]
    public class DynamicsOfKeyIndicatorsRecord
    {
        public int Id { get; set; }



        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
