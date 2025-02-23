using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    //TODO
    [Table("LiteratureWork")]
    public class LiteratureWorkRecord
    {
        public int Id { get; set; }



        public string? ShortAnnotation { get; set; } = string.Empty;

        public int JournalId { get; set; }
        public Journal? Journal;
    }
}
