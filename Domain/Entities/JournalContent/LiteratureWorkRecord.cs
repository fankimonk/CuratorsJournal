using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent
{
    //TODO
    [Table("LiteratureWork")]
    public class LiteratureWorkRecord
    {
        public int Id { get; set; }



        public string? ShortAnnotation { get; set; } = string.Empty;

        public int PageId { get; set; }
        public Page? Page;
    }
}
