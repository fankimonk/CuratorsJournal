using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.StudentHealthCards
{
    public record UpdateHealthCardRecordRequest
    (
        [Range(1, int.MaxValue, ErrorMessage = "Номер должен быть больше 0")]
        int? Number,
        [Range(0, int.MaxValue)]
        int? MissedClasses,

        string? Note,
        int? StudentId
    );
}
