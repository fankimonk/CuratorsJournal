using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent.Literature
{
    [Table("LiteratureList")]
    public class LiteratureListRecord
    {
        public int Id { get; set; }

        public string Author { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string BibliographicData { get; set; } = string.Empty;

        public List<LiteratureWorkRecord> LiteratureWorkRecords { get; set; } = [];
    }
}
