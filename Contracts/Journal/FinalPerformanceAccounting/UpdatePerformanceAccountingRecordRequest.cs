using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.FinalPerformanceAccounting
{
    public record UpdatePerformanceAccountingRecordRequest
    (
        [Range(1, int.MaxValue, ErrorMessage = "Номер должен быть больше 0")]
        int? Number,
        int? StudentId
    );
}
