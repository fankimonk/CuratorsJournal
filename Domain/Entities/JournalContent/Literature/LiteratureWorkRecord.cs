using Domain.Entities.JournalContent.Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent.Literature
{
    [Table("LiteratureWork")]
    public class LiteratureWorkRecord
    {
        public int Id { get; set; }

        public int? LiteratureId { get; set; }
        public LiteratureListRecord? Literature { get; set; }

        public string? ShortAnnotation { get; set; } = string.Empty;

        public int PageId { get; set; }
        public Page? Page;
    }
}
