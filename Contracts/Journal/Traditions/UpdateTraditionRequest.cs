using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.Traditions
{
    public record UpdateTraditionRequest
    (
        string? Name,
        string? ParticipationForm,
        string? Note,

        [Range(1, 31, ErrorMessage = "День должен быть между 1 и 31")]
        int? Day,
        [Range(1, 12)]
        int? Month
    );
}
