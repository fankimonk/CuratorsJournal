using System.ComponentModel.DataAnnotations;

namespace Contracts.Literature
{
    public record UpdateLiteratureRequest
    (
        [Required]
        string Author,
        [Required]
        string Name,
        [Required]
        string BibliographicData
    );
}
