using System.ComponentModel.DataAnnotations;

namespace Contracts.Literature
{
    public class UpdateLiteratureRequest
    (
        string author,
        string name,
        string bibliographicData
    )
    {
        [Required]
        public string Author { get; set; } = author;

        [Required]
        public string Name { get; set; } = name;

        [Required]
        public string BibliographicData { get; set; } = bibliographicData;
    }
}
