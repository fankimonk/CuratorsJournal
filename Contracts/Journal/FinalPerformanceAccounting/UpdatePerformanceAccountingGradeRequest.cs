using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.FinalPerformanceAccounting
{
    public record UpdatePerformanceAccountingGradeRequest
    (
        bool? IsPassed,

        [Range(0, 10, ErrorMessage = "Оценка должна быть между 0 и 10")]
        int? Grade
    );
}
